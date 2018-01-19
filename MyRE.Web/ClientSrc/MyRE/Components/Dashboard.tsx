import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Instance, User } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { Option } from 'ts-option';
import { NeedsInitializationAlert } from 'MyRE/Components/NeedsInitializationAlert';
import { PageHeader } from 'MyRE/Components/PageHeader';
import { Container } from 'reactstrap';

interface IOwnProps { }
interface IConnectedState {
    instances: Option<List<Instance>>;
    user: Option<User>;
}
interface IConnectedDispatch {
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    instances: state.instanceState.instances,
    user: state.auth.currentUser,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

type DashboardProps = IOwnProps & IConnectedDispatch & IConnectedState & RouteComponentProps<IOwnProps & IConnectedDispatch & IConnectedState>;

class DashboardComponent extends React.Component<DashboardProps> {
    public render() {
        if (this.props.instances.isDefined && this.props.instances.get.count() < 1) {
            return <NeedsInitializationAlert />;
        }

        return (
            <Container>
                <PageHeader>Dashboard</PageHeader>
            </Container>);
    }
}

export const Dashboard = connect(mapStateToProps, mapDispatchToProps)(DashboardComponent);
