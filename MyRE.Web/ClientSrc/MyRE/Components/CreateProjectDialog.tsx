import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { Modal, ModalHeader, ModalBody, ModalFooter, Container, FormGroup, Label, Col, Input, Button } from 'reactstrap';
import { toggleCreateProjectDialog, changeNewProjectData, createNewProject } from 'MyRE/Actions/Projects';
import { Instance, CreateProjectRequest } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { AlertRow } from 'MyRE/Components/AlertRow';
import { Option } from 'ts-option';

interface IOwnProps { }
interface IConnectedState {
    isOpen: boolean;
    instances: List<Instance>;
    newProject: CreateProjectRequest;
    alertMessage: Option<Store.AlertMessage>;
}

interface IConnectedDispatch {
    toggleModal: () => void;
    changeNewProjectProperties: (name: string, description: string, instanceId: string) => void;
    submitProjectCreate: (project: CreateProjectRequest) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    isOpen: state.projects.createProjectModalOpen,
    instances: state.instanceState.instances.getOrElse(List([])),
    newProject: state.projects.newProject,
    alertMessage: state.projects.createProjectMessage,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    toggleModal: () => {
        dispatch(toggleCreateProjectDialog());
    },
    changeNewProjectProperties: (name, description, instanceId) => {
        dispatch(changeNewProjectData({
            Name: name,
            Description: description,
            InstanceId: instanceId,
        }));
    },
    submitProjectCreate: (project) => {
        dispatch(createNewProject(project));
    }
});

class CreateProjectDialogComponent extends React.PureComponent<IConnectedState & IConnectedDispatch & IOwnProps> {
    private changeName: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.changeNewProjectProperties(e.target.value, this.props.newProject.Description, this.props.newProject.InstanceId);
    }

    private changeDescription: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.changeNewProjectProperties(this.props.newProject.Name, e.target.value, this.props.newProject.InstanceId);
    }

    private changeInstanceId: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.changeNewProjectProperties(this.props.newProject.Name, this.props.newProject.Description, e.target.value);
    }

    private submit = () => {
        this.props.submitProjectCreate(this.props.newProject);
    }

    public render() {

        if (this.props.instances.count() === 1) {
            if (this.props.instances.first().InstanceId != this.props.newProject.InstanceId) {
                Promise.resolve()
                    .then(_ => this.props.changeNewProjectProperties(this.props.newProject.Name, this.props.newProject.Description, this.props.instances.first().InstanceId));                
            }
        }

        return (
            <Modal isOpen={this.props.isOpen} toggle={this.props.toggleModal}>
                <ModalHeader toggle={this.props.toggleModal}>Create New Project</ModalHeader>
                <ModalBody>
                    <Container>
                        <AlertRow message={this.props.alertMessage} />
                        <FormGroup row>
                            <Label for="new-project-name" xs="12" md="3">Name</Label>
                            <Col xs="12" md="9">
                                <Input type="text" name="new-project-name"
                                    value={this.props.newProject.Name}
                                    onChange={this.changeName} />
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Label for="new-project-description" xs="12" md="3">Description</Label>
                            <Col xs="12" md="9">
                                <Input type="textarea" name="new-project-description"
                                    value={this.props.newProject.Description}
                                    onChange={this.changeDescription} />
                            </Col>
                        </FormGroup>
                        {
                            this.props.instances.count() > 1 &&
                            <FormGroup row>
                                <Label for="exampleSelect" xs={12} md={3}>Instance</Label>
                                <Col xs={12} sm={9}>
                                    <Input type="select" name="new-project-instance">
                                    </Input>
                                </Col>
                            </FormGroup>
                        }
                    </Container>
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={this.submit}>Create</Button>
                </ModalFooter>
            </Modal>
        );
    }
}

export const CreateProjectDialog =
    connect(mapStateToProps, mapDispatchToProps)(
        CreateProjectDialogComponent);
