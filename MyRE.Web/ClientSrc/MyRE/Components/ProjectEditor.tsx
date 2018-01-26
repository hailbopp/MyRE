import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

import { List } from 'immutable';
import { Row, Col } from 'reactstrap';
import AceEditor from 'react-ace';
import brace = require('brace');

//const Parser = require('MyRE/Utils/MyreLisp.pegjs')
import * as Parser from 'MyRE/Utils/MyreLisp.pegjs';

import 'brace/mode/lisp';
import 'brace/theme/kuroir';

interface IOwnProps {
    project: Store.Project;
}

interface IConnectedState {
}

interface IConnectedDispatch {
}

export type IProjectEditorProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});


class ProjectEditorComponent extends React.PureComponent<IProjectEditorProperties> {
    private onChangeHandler = (value: string) => {

    }

    public render() {
        console.log(Parser);

        return (
            <Row>
                <AceEditor
                    mode="lisp"
                    theme="kuroir"
                    width="100%"
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
