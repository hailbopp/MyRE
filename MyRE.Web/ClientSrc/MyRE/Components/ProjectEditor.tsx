import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

import { List } from 'immutable';
import { Row, Col } from 'reactstrap';
import AceEditor from 'react-ace';
import brace = require('brace');

import 'brace/ext/language_tools'

import MyreLispAceMode, { MyreLispCompletions } from 'MyRE/Utils/Ace/MyreLispAceMode';

import 'brace/theme/kuroir';
import { changeProjectSource } from 'MyRE/Actions/Projects';
import { DeviceInfo } from 'MyRE/Api/Models';

interface IOwnProps {
    project: Store.Project;
}

interface IConnectedState {
    availableDevices: DeviceInfo[];
}

interface IConnectedDispatch {
    changeSource: (projectId: string, newSource: string) => void;
}

export type IProjectEditorProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    availableDevices: state.instanceState.instances.map(il => il.find(inst => !!inst && inst.instanceId === ownProps.project.instanceId).devices.getOrElse(List([])).toArray()).getOrElse([])
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    changeSource: (projectId, newSource) => {
        dispatch(changeProjectSource(projectId, newSource));
    }
});

class ProjectEditorComponent extends React.PureComponent<IProjectEditorProperties> {
    private aceEditor?: brace.Editor;

    private onChangeHandler = (value: string) => {
        this.props.changeSource(this.props.project.projectId, value);
    }

    private setCompletionHandler() {
        const myrelispCompleter = new MyreLispCompletions(this.props.availableDevices);
        const langTools = brace.acequire('ace/ext/language_tools');
        langTools.setCompleters([myrelispCompleter]);
    }

    public componentDidMount() {
        const myrelispMode = new MyreLispAceMode();        
        
        if (this.aceEditor) {            
            this.aceEditor.getSession().setMode(myrelispMode);
            this.setCompletionHandler();

            this.aceEditor.setOptions({
                enableBasicAutocompletion: true,
                enableLiveAutocompletion: true
            });
        }        
    }

    public componentDidUpdate(prevProps: IProjectEditorProperties) {
        if (this.props.availableDevices.length !== prevProps.availableDevices.length) {
            this.setCompletionHandler();
        }
    }

    public render() {
        return (
            <Row>
                <AceEditor
                    ref={(ref) => { if (ref) this.aceEditor = (ref as any).editor as brace.Editor }}
                    mode={"text"}
                    theme="kuroir"
                    width="100%"
                    value={this.props.project.source.Source}
                    onChange={this.onChangeHandler}
                    name={"editor" + this.props.project.projectId}
                    editorProps={{ $blockScrolling: true }}
                />
            </Row>
        );
    }
}

export const ProjectEditor =
    connect(mapStateToProps, mapDispatchToProps)(
        ProjectEditorComponent);
