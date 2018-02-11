import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

import { List } from 'immutable';
import { Row, Col } from 'reactstrap';
import AceEditor from 'react-ace';
import brace = require('brace');

import 'brace/ext/language_tools'

import MyreLispAceMode from 'MyRE/Utils/Ace/MyreLispAceMode';

import 'brace/theme/kuroir';
import { changeProjectSource } from 'MyRE/Actions/Projects';

interface IOwnProps {
    project: Store.Project;
}

interface IConnectedState {
}

interface IConnectedDispatch {
    changeSource: (projectId: string, newSource: string) => void;
}

export type IProjectEditorProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
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

    public componentDidMount() {
        const myrelispMode = new MyreLispAceMode();        
        if (this.aceEditor) {
            const langTools = brace.acequire('ace/ext/language_tools');

            //langTools.setCompleters([]);
            this.aceEditor.getSession().setMode(myrelispMode);
            (window as any).langtools = langTools;
            langTools.setCompleters([myrelispMode.$completer]);

            this.aceEditor.setOptions({
                enableBasicAutocompletion: true,
                enableLiveAutocompletion: true
            });
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
