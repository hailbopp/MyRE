import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Container, Table, Row, Col, Button } from 'reactstrap';
import { ProjectListing } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { Option } from 'ts-option';
import { requestProjectList, toggleCreateProjectDialog, deleteProject } from 'MyRE/Actions/Projects';
import { CreateProjectDialog } from 'MyRE/Components/CreateProjectDialog';

interface IOwnProps { }
interface IConnectedState {
    projects: Option<List<ProjectListing>>;
    retrievingProjects: boolean;
}
interface IConnectedDispatch {
    getProjects: () => void;
    toggleCreateModal: () => void;
    deleteProject: (projectId: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projects: state.projects.projects,
    retrievingProjects: state.projects.retrievingProjects,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    getProjects: () => {
        dispatch(requestProjectList());
    },
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

    public componentWillMount() {
        if (!this.props.retrievingProjects && this.props.projects.isEmpty) {
            this.props.getProjects();
        }
    }

    public render() {
        return (
            <div>
                <Container>
                    <Row>
                        <Col xs="12" sm="10">
                            <h4>Projects</h4>
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
                                        <td>{p.Name}</td>
                                        <td>{p.Description}</td>
                                        <td>
                                            <span className="float-right">
                                                <Button outline size="sm" color="primary" onClick={() => this.props.history.push(`/projects/${p.Id}/edit`)}>Edit</Button>
                                                {' '}
                                                <Button outline size="sm" color="danger" onClick={() => this.props.deleteProject(p.Id)}>Delete</Button>
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
