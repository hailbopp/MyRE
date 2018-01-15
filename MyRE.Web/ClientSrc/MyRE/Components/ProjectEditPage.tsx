import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { RouteComponentProps, withRouter } from 'react-router';

interface IOwnProps {}
interface IConnectedState {
}

interface IConnectedDispatch {
}

export type IProjectEditPageProperties = IOwnProps & IConnectedState & IConnectedDispatch & RouteComponentProps<{ projectId: string; }>;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

class ProjectEditPageComponent extends React.PureComponent<IProjectEditPageProperties> {
	public render() {
		return (
            <div>
                Editing project {this.props.match.params.projectId}
			</div>
		);
	}
}

export const ProjectEditPage =
    withRouter(
    connect(mapStateToProps, mapDispatchToProps)(
        ProjectEditPageComponent));
