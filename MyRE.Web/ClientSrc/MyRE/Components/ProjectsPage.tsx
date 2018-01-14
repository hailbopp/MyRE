import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Container, Table, Row, Col } from 'reactstrap';
import { ProjectListing } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { Option } from 'ts-option';
import { requestProjectList } from 'MyRE/Actions/Projects';

interface IOwnProps { }
interface IConnectedState {
    projects: Option<List<ProjectListing>>;
    retrievingProjects: boolean;
}
interface IConnectedDispatch {
    getProjects: () => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projects: state.projects.projects,
    retrievingProjects: state.projects.retrievingProjects,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    getProjects: () => {
        dispatch(requestProjectList());
    }
});

class ProjectsPageComponent extends React.Component<IOwnProps & IConnectedDispatch & IConnectedState & RouteComponentProps<IOwnProps & IConnectedDispatch & IConnectedState>> {
    public render() {
        if (!this.props.retrievingProjects && this.props.projects.isEmpty) {
            this.props.getProjects();
        }

        return (
            <Container>
                <Row>
                    <Col>
                        <h4>Projects</h4>
                    </Col>
                </Row>
                
                <Table>
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            
                        }
                    </tbody>
                </Table>
            </Container>
            );
    }
}

export const ProjectsPage = connect(mapStateToProps, mapDispatchToProps)(ProjectsPageComponent);
