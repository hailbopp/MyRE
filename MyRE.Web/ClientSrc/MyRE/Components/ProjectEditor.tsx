import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

import { List } from 'immutable';
import { Row, Col } from 'reactstrap';

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
    public render() {

        return (
            <Row>
            </Row>
        );
    }
}

export const ProjectEditor =
    connect(mapStateToProps, mapDispatchToProps)(
        ProjectEditorComponent);
