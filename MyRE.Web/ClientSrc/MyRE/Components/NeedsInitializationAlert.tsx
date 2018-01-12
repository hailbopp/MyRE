import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { Container } from 'reactstrap';
import { Option, some } from 'ts-option';
import { AlertRow } from 'MyRE/Components/AlertRow';

interface IOwnProps { }
interface IConnectedState {
}
interface IConnectedDispatch {
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

class NeedsInitializationAlertComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch> {
    public render() {
        let msg: Store.AlertMessage = {
            level: 'warning',
            message: "You have no registered instances on your account. Using the SmartThings app, open the SmartApp instance that you wish to register, and click 'Web Console'.",
        }

        return (
            <Container>
                <AlertRow message={some(msg)} />
            </Container>
        );
    }
}

export const NeedsInitializationAlert = connect(mapStateToProps, mapDispatchToProps)(NeedsInitializationAlertComponent);