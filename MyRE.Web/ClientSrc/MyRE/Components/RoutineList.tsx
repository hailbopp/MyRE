import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { List } from 'immutable';

interface IOwnProps {
    routines: List<Store.Routine>;
}

interface IConnectedState {
}

interface IConnectedDispatch {
}

export type IRoutineListProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});



class RoutineListComponent extends React.PureComponent<IRoutineListProperties> {
	public render() {
		return (
			<div>
			</div>
		);
	}
}

export const RoutineList =
    connect(mapStateToProps, mapDispatchToProps)(
        RoutineListComponent);
