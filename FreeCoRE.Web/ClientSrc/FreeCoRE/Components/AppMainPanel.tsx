import * as React from 'react';
import { Provider, connect, Dispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { Store } from 'FreeCoRE/Models/Store';
import { Route } from 'react-router';
import { Dashboard } from 'FreeCoRE/Components/Dashboard';

interface IOwnProps { }
interface IConnectedState { }
interface IConnectedDispatch { }

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

class AppMainPanelComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch> {
    public render() {
        return (
            <div>
                <Route exact path="/" component={Dashboard} />
            </div>
        );
    }
}

export const AppMainPanel = connect(mapStateToProps, mapDispatchToProps)(AppMainPanelComponent);