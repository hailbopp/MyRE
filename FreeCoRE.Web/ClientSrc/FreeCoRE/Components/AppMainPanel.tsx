import * as React from 'react';
import { Provider, connect, Dispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { Store } from 'FreeCoRE/Models/Store';
import { Route, Redirect, Switch, withRouter, RouteComponentProps } from 'react-router';
import { Dashboard } from 'FreeCoRE/Components/Dashboard';
import { Option } from 'ts-option';
import { retrieveCurrentUser } from 'FreeCoRE/Actions/Auth';
import { LoginPage } from 'FreeCoRE/Components/LoginPage';

interface IOwnProps { }
interface IConnectedState {
    isLoggedIn: Option<boolean>;
}
interface IConnectedDispatch {
    initiateCheckForLoggedInUser: () => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    isLoggedIn: state.auth.isLoggedIn,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    initiateCheckForLoggedInUser: () => {
        dispatch(retrieveCurrentUser());
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

    public render() {
        const authStatus = this.getAuthStatus(this.props.isLoggedIn);

        if (authStatus === AuthenticationStatus.ShouldCheckForAuth) {
            this.props.initiateCheckForLoggedInUser();
        }

        return (
            <Switch>
                <Route path="/login" component={LoginPage} />
                {authStatus === AuthenticationStatus.NotAuthenticated && <Redirect to="/login" />}

                <Route exact path="/" component={Dashboard} />
            </Switch>
        );
    }
}

export const AppMainPanel =
    withRouter( 
    connect(mapStateToProps, mapDispatchToProps)(
        AppMainPanelComponent
    ));