import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { Switch, Route, RouteComponentProps, withRouter } from 'react-router';
import { Container, Row, Col, Button } from 'reactstrap';
import { PageHeader } from 'MyRE/Components/PageHeader';
import { ProjectListing, DeviceInfo } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { ProjectEditor } from 'MyRE/Components/ProjectEditor';
import { setActiveProject, updateProject } from 'MyRE/Actions/Projects';
import { Option, none, some } from 'ts-option';
import { filterDevices } from 'MyRE/Utils/Helpers/Instance';

type IOwnProps = RouteComponentProps<{ projectId: string; }>;
interface IConnectedState {
    projects: Option<List<Store.Project>>;
    activeProject: Option<Store.ActiveProject>;
    activeProjectAvailableDevices: Option<DeviceInfo[]>
}

interface IConnectedDispatch {
    setActive: (projectId: string, availableDevices: DeviceInfo[]) => void;
    updateActiveProject: (entity: Store.Project) => void;
}

export type IIndividualProjectPageProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projects: state.projects.projects,
    activeProject: state.projects.activeProject,
    activeProjectAvailableDevices: state.projects.activeProject.isDefined ? some(filterDevices(state.projects.activeProject.get.internal.instanceId, state.instanceState).toArray()) : none,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    setActive: (projectId, availableDevices) => {
        dispatch(setActiveProject(projectId, availableDevices));
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

class IndividualProjectPageComponent extends React.Component<IIndividualProjectPageProperties> {
    private checkActiveProject() {
        if (this.props.projects.isDefined && (this.props.activeProject.isEmpty || this.props.match.params.projectId !== this.props.activeProject.get.internal.projectId)) {
            this.props.setActive(this.props.match.params.projectId, this.props.activeProjectAvailableDevices.getOrElse([]));
        }
    }

    public componentWillUpdate() {
        this.checkActiveProject();
    }

    public componentDidMount() {
        this.checkActiveProject();
    }
    
    public render() {

        if (this.props.activeProject.isEmpty || this.props.match.params.projectId !== this.props.activeProject.get.internal.projectId) {
            return <Container/>;
        }

        const proj = this.props.activeProject.get;

        return (
            <Container>
                <Row>
                    <Col xs="12" sm="10">
                        <PageHeader>{proj.display.name}</PageHeader>
                    </Col>
                    <Col xs="12" sm="2">
                        <Button color="primary" className="float-right" size="sm" onClick={() => this.props.updateActiveProject(proj.internal)}>Save</Button>
                    </Col>
                </Row>

                <ProjectEditor project={proj} />
            </Container>
        );
    }
}

export const IndividualProjectPage =
    withRouter(
        connect(mapStateToProps, mapDispatchToProps)(
            IndividualProjectPageComponent));
