import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Instance, User } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { Option } from 'ts-option';
import { listUserInstances } from 'MyRE/Actions/Instances';
import { NeedsInitializationAlert } from 'MyRE/Components/NeedsInitializationAlert';

interface IOwnProps { }
interface IConnectedState {
    instances: Option<List<Instance>>;
    user: Option<User>;
}
interface IConnectedDispatch {
    retrieveInstanceList: (userId: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    instances: state.instanceState.instances,
    user: state.auth.currentUser,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    retrieveInstanceList: (userId: string) => {
        dispatch(listUserInstances(userId));
    }
});

type DashboardProps = IOwnProps & IConnectedDispatch & IConnectedState & RouteComponentProps<IOwnProps & IConnectedDispatch & IConnectedState>;

class DashboardComponent extends React.Component<DashboardProps> {
    public render() {
        let loading = false;

        if (this.props.instances.isEmpty && this.props.user.isDefined) {
            this.props.retrieveInstanceList(this.props.user.get.UserId);
            loading = true;
        }

        if (loading) {
            return <div>Loading...</div>;
        }

        if (this.props.instances.isDefined && this.props.instances.get.count() < 1) {
            return <NeedsInitializationAlert />;
        }

        return (
            <div>
                Dashboard
            </div>);
    }
}

export const Dashboard = connect(mapStateToProps, mapDispatchToProps)(DashboardComponent);
