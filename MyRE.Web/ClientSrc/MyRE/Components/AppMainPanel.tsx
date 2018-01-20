import * as React from 'react';
import { Provider, connect, Dispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { Store } from 'MyRE/Models/Store';
import { Route, Redirect, Switch, withRouter, RouteComponentProps } from 'react-router';
import { Dashboard } from 'MyRE/Components/Dashboard';
import { Option } from 'ts-option';
import { retrieveCurrentUser } from 'MyRE/Actions/Auth';
import { LoginPage } from 'MyRE/Components/LoginPage';
import { Instance } from 'MyRE/Api/Models';
import { List } from 'immutable';
import { listUserInstances } from 'MyRE/Actions/Instances';
import { Projects } from 'MyRE/Components/Projects';

interface IOwnProps { }
interface IConnectedState {
    instances: Option<List<Instance>>;
    instancesLoading: boolean;
    isLoggedIn: Option<boolean>;
    userId: Option<string>;
}
interface IConnectedDispatch {
    initiateCheckForLoggedInUser: () => void;
    retrieveInstanceList: (userId: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    instances: state.instanceState.instances,
    instancesLoading: state.asyncActions.currentAsyncActions.toArray().some(a => a.type === 'API_REQUEST_USER_INSTANCE_LIST'),
    isLoggedIn: state.auth.isLoggedIn,
    userId: state.auth.currentUser.map(u => u.UserId),
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    initiateCheckForLoggedInUser: () => {
        dispatch(retrieveCurrentUser());
    },
    retrieveInstanceList: (userId) => {
        dispatch(listUserInstances(userId));
    }
});

enum AuthenticationStatus {
    ShouldCheckForAuth,
    LoggedIn,
    NotAuthenticated,
}

class AppMainPanelComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch & RouteComponentProps<{}>, {}> {
    private getAuthStatus = (isLoggedIn: Option<boolean>): AuthenticationStatus => {
        if (isLoggedIn.isEmpty) {
            return AuthenticationStatus.ShouldCheckForAuth;
        } else if (isLoggedIn.isDefined && isLoggedIn.get === true) {
            return AuthenticationStatus.LoggedIn;
        } else {
            return AuthenticationStatus.NotAuthenticated;
        }
    }

    public componentWillMount() {
        const authStatus = this.getAuthStatus(this.props.isLoggedIn);
        if (authStatus === AuthenticationStatus.ShouldCheckForAuth) {
            this.props.initiateCheckForLoggedInUser();
        }
    }

    public componentDidUpdate() {
        if (this.props.instances.isEmpty && this.props.userId.isDefined && !this.props.instancesLoading) {
            this.props.retrieveInstanceList(this.props.userId.get);
        }
    }

    public render() {
        const authStatus = this.getAuthStatus(this.props.isLoggedIn);
        return (
            <Switch>
                <Route path="/login" component={LoginPage} />
                {authStatus === AuthenticationStatus.NotAuthenticated &&
                    <Redirect to="/login" />}                

                <Route exact path="/" component={Dashboard} />
                {this.props.instances.isDefined && this.props.instances.get.isEmpty() &&
                    <Redirect to="/" />}
                
                <Route path="/projects" component={Projects} />
            </Switch>
        );
    }
}

export const AppMainPanel =
    withRouter( 
    connect(mapStateToProps, mapDispatchToProps)(
        AppMainPanelComponent
    ));