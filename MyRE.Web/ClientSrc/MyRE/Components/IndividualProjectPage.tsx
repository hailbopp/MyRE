import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { Switch, Route, RouteComponentProps, withRouter } from 'react-router';
import { Container, Row, Col, Button } from 'reactstrap';
import { PageHeader } from 'MyRE/Components/PageHeader';
import { ProjectListing } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { ProjectEditor } from 'MyRE/Components/ProjectEditor';
import { setActiveProject, updateProject } from 'MyRE/Actions/Projects';
import { Option } from 'ts-option';

type IOwnProps = RouteComponentProps<{ projectId: string; }>;
interface IConnectedState {
    projects: Option<List<Store.Project>>;
    activeProject: Option<Store.Project>;
}

interface IConnectedDispatch {
    setActive: (projectId: string) => void;
    updateActiveProject: (entity: Store.Project) => void;
}

export type IIndividualProjectPageProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projects: state.projects.projects,
    activeProject: state.projects.activeProject,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    setActive: projectId => {
        dispatch(setActiveProject(projectId));
    },
    updateActiveProject: entity => {
        dispatch(updateProject({
            ProjectId: entity.projectId,
            Name: entity.name,
            Description: entity.description,
            InstanceId: entity.instanceId,
            Source: entity.source
        }));
    }
});

class IndividualProjectPageComponent extends React.PureComponent<IIndividualProjectPageProperties> {
    private checkActiveProject() {
        if (this.props.projects.isDefined && (this.props.activeProject.isEmpty || this.props.match.params.projectId !== this.props.activeProject.get.projectId)) {
            this.props.setActive(this.props.match.params.projectId);
        }
    }

    public componentWillUpdate() {
        this.checkActiveProject();
    }

    public componentDidMount() {
        this.checkActiveProject();
    }

    public render() {
        if (this.props.activeProject.isEmpty || this.props.match.params.projectId !== this.props.activeProject.get.projectId) {
            return <Container/>;
        }

        const proj = this.props.activeProject.get;

        return (
            <Container>
                <Row>
                    <Col xs="12" sm="10">
                        <PageHeader>{proj.name}</PageHeader>
                    </Col>
                    <Col xs="12" sm="2">
                        <Button color="primary" className="float-right" size="sm" onClick={() => this.props.updateActiveProject(proj)}>Save</Button>
                    </Col>
                </Row>

                <ProjectEditor project={proj} />
            </Container>
        );
    }
}

export const IndividualProjectPage =
    connect(mapStateToProps, mapDispatchToProps)(
        withRouter(
            IndividualProjectPageComponent));
