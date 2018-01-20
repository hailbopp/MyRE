import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { List } from 'immutable';
import { Table } from 'reactstrap';
import { RoutineListRow } from 'MyRE/Components/RoutineListRow';

interface IOwnProps {
    routines: List<Store.Routine>;
}

export type IRoutineListProperties = IOwnProps;

class RoutineListComponent extends React.PureComponent<IRoutineListProperties> {
	public render() {
		return (
            <Table responsive>
                {/*<thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th />
                    </tr>
                </thead>*/}
                <tbody>
                    {
                        this.props.routines.toArray().map(r => <RoutineListRow routine={r} key={r.routineId}/>)
                    }
                </tbody>
            </Table>
		);
	}
}

export const RoutineList = RoutineListComponent;
