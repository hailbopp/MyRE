import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

import { List } from 'immutable';
import { Row, Col, Container, Button } from 'reactstrap';
import AceEditor from 'react-ace';
import brace = require('brace');

import 'brace/ext/language_tools'

import MyreLispAceMode, { MyreLispCompletions } from 'MyRE/Utils/Ace/MyreLispAceMode';

import 'brace/theme/kuroir';
import { changeProjectSource, testProjectSource } from 'MyRE/Actions/Projects';
import { DeviceInfo, ProjectSource } from 'MyRE/Api/Models';
import { filterDevices } from 'MyRE/Utils/Helpers/Instance';
import { convertInternalSourceToDisplayFormat, convertDisplaySourceToInternalFormat } from 'MyRE/Utils/Helpers/Project';
import { AlertRow } from 'MyRE/Components/AlertRow';

interface IOwnProps {
    project: Store.ActiveProject;
}

interface IConnectedState {
    availableDevices: DeviceInfo[];
    projectId: string;
}

interface IConnectedDispatch {
    changeSource: (projectId: string, devices: DeviceInfo[], newSource: string) => void;
    testSource: (instanceId: string, source: string) => void;
}

export type IProjectEditorProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    availableDevices: filterDevices(ownProps.project.internal.instanceId, state.instanceState).toArray(),
    projectId: ownProps.project.internal.projectId
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    changeSource: (projectId, devices, newSource) => {
        dispatch(changeProjectSource(projectId, devices, newSource));
    },
    testSource: (instanceId, source) => {
        dispatch(testProjectSource(instanceId, source));
    }
});

class ProjectEditorComponent extends React.PureComponent<IProjectEditorProperties> {
    private aceEditor?: brace.Editor;

    private onChangeHandler = (value: string) => {
        this.props.changeSource(this.props.projectId, this.props.availableDevices, value);
    }

    private onTestButtonClickHandler = () => {
        this.props.testSource(this.props.project.internal.instanceId, this.props.project.internal.source.Source);
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
            <Container>
                <Row>
                    <AlertRow message={this.props.project.editorStatusMessage} />
                </Row>
                <Row>
                    <AceEditor
                        ref={(ref) => { if (ref) this.aceEditor = (ref as any).editor as brace.Editor }}
                        mode={"text"}
                        theme="kuroir"
                        width="100%"
                        value={this.props.project.display.source.Source}
                        onChange={this.onChangeHandler}
                        name={"editor" + this.props.project.display.projectId}
                        editorProps={{ $blockScrolling: true }}
                    />
                </Row>
                <Row>
                    <Col xs="12" sm="2">
                        <Button color="primary" className="float-right" size="sm" onClick={this.onTestButtonClickHandler}>Test</Button>
                    </Col>
                </Row>
            </Container>
        );
    }
}

export const ProjectEditor =
    connect(mapStateToProps, mapDispatchToProps)(
        ProjectEditorComponent);
