import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

interface IOwnProps {
    routine: Store.Routine;
}
interface IConnectedState {
}

interface IConnectedDispatch {
}

export type IRoutineListRowProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

class RoutineListRowComponent extends React.PureComponent<IRoutineListRowProperties> {
    public render() {
        return (
            <tr>
                <td>
                    {this.props.routine.name}
                </td>
            </tr>
        );
    }
}

export const RoutineListRow =
    connect(mapStateToProps, mapDispatchToProps)(
        RoutineListRowComponent);
