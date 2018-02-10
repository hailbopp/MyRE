import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

import { List } from 'immutable';
import { Row, Col } from 'reactstrap';
import AceEditor from 'react-ace';
import brace = require('brace');

import 'brace/mode/lisp';
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
    private onChangeHandler = (value: string) => {
        this.props.changeSource(this.props.project.projectId, value);
    }

    public render() {
        return (
            <Row>
                <AceEditor
                    mode="lisp"
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
