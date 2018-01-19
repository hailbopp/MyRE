import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps, Route } from 'react-router';
import { Container, Table, Row, Col, Button } from 'reactstrap';
import { List } from 'immutable';
import { Option } from 'ts-option';
import { toggleCreateProjectDialog, deleteProject } from 'MyRE/Actions/Projects';
import { CreateProjectDialog } from 'MyRE/Components/CreateProjectDialog';
import { PageHeader } from 'MyRE/Components/PageHeader';

interface IOwnProps { }
interface IConnectedState {
    projects: Option<List<Store.Project>>;
}
interface IConnectedDispatch {
    toggleCreateModal: () => void;
    deleteProject: (projectId: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projects: state.projects.projects,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    toggleCreateModal: () => {
        dispatch(toggleCreateProjectDialog());
    },
    deleteProject: (projectId) => {
        dispatch(deleteProject(projectId));
    }
});

class ProjectsPageComponent extends React.Component<IOwnProps & IConnectedDispatch & IConnectedState & RouteComponentProps<IOwnProps & IConnectedDispatch & IConnectedState>> {
    private deleteProject = (projectId: string) => {
        return () => {
            this.props.deleteProject(projectId);
        }
    }
    
    public render() {
        return (
            <div>
                <Container>
                    <Row>
                        <Col xs="12" sm="10">
                            <PageHeader>Projects</PageHeader>
                        </Col>
                        <Col xs="12" sm="2">
                            <Button color="primary" className="float-right" size="sm" onClick={this.props.toggleCreateModal}>New Project</Button>
                        </Col>
                    </Row>
                    <Table responsive>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th />
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.props.projects.isDefined &&
                                this.props.projects.get.toArray().map((p, idx) =>
                                    <tr key={idx}>
                                        <td>{p.name}</td>
                                        <td>{p.description}</td>
                                        <td>
                                            <span className="float-right">
                                                <Button outline size="sm" color="primary" onClick={() => this.props.history.push(`/projects/${p.projectId}`)}>Edit</Button>
                                                {' '}
                                                <Button outline size="sm" color="danger" onClick={() => this.props.deleteProject(p.projectId)}>Delete</Button>
                                            </span>
                                        </td>
                                    </tr>)
                            }
                        </tbody>
                    </Table>
                </Container>
                <CreateProjectDialog />
            </div>
        );
    }
}

export const ProjectsPage = connect(mapStateToProps, mapDispatchToProps)(ProjectsPageComponent);
