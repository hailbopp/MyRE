import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { Switch, Route } from 'react-router';
import { ProjectsPage } from 'MyRE/Components/ProjectsPage';
import { IndividualProjectPage } from 'MyRE/Components/IndividualProjectPage';
import { List } from 'immutable';
import { Option } from 'ts-option';
import { requestProjectList } from 'MyRE/Actions/Projects';

interface IOwnProps {}
interface IConnectedState {
    projects: Option<List<Store.Project>>;
    retrievingProjects: boolean;
}

interface IConnectedDispatch {
    getProjects: () => void;
}

export type IProjectsProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projects: state.projects.projects,
    retrievingProjects: state.projects.retrievingProjects,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    getProjects: () => {
        dispatch(requestProjectList());
    },
});

class ProjectsComponent extends React.PureComponent<IProjectsProperties> {
    public componentWillMount() {
        if (!this.props.retrievingProjects && this.props.projects.isEmpty) {
            this.props.getProjects();
        }
    }

	public render() {
		return (
            <Switch>
                <Route exact path="/projects" component={ProjectsPage} />
                <Route path="/projects/:projectId" component={IndividualProjectPage} />
            </Switch>
		);
	}
}

export const Projects =
    connect(mapStateToProps, mapDispatchToProps)(
        ProjectsComponent);
