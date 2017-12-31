import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store as ReduxStore } from 'redux';
import { Store } from 'FreeCoRE/Models/Store';
import { Link } from 'react-router-dom';
import { Route, withRouter, RouteComponentProps } from 'react-router';
import { Dashboard } from 'FreeCoRE/Components/Dashboard';
import { Navbar } from 'FreeCoRE/Components/Navbar';
import { AppMainPanel } from 'FreeCoRE/Components/AppMainPanel';
import { Option } from 'ts-option';
import { retrieveCurrentUser } from 'FreeCoRE/Actions/Auth';

interface IOwnProps {}
interface IConnectedState {
}
interface IConnectedDispatch {
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

class ApplicationComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch & RouteComponentProps<{}>> {
    public render() {
        return (
            <div>
                <Navbar />
                <AppMainPanel />
            </div>
        );
    }
}

export const Application =
    withRouter(
        connect(mapStateToProps, mapDispatchToProps)(
            ApplicationComponent));