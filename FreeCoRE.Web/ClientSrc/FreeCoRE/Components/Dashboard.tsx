import * as React from 'react';
import { Store } from 'FreeCoRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { InputGroup, InputGroupAddon, Input } from 'reactstrap';

interface IOwnProps { }
interface IConnectedState { }
interface IConnectedDispatch { }

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

type DashboardProps = IOwnProps & IConnectedDispatch & IConnectedState & RouteComponentProps<IOwnProps & IConnectedDispatch & IConnectedState>;

class DashboardComponent extends React.Component<DashboardProps> {
    public render() {
        return (
            <div>
                Dashboard
            </div>
        );
    }
}

export const Dashboard = connect(mapStateToProps, mapDispatchToProps)(DashboardComponent);
