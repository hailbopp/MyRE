import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import * as brace from 'brace';
import AceEditor from 'react-ace';

import 'brace/theme/kuroir';
import 'brace/mode/javascript';
import { List } from 'immutable';

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
            <div>
                <AceEditor mode="javascript" theme="kuroir" name="editor-{this.props.project.projectId}"
                    width="100%"
                    value={this.props.project.projectId} />
			</div>
		);
	}
}

export const ProjectEditor =
    connect(mapStateToProps, mapDispatchToProps)(
        ProjectEditorComponent);
