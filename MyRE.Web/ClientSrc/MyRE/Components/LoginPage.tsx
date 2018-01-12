import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'reactstrap';
import { LoginForm } from 'MyRE/Components/LoginForm';
import { RegistrationForm } from 'MyRE/Components/RegistrationForm';
import { Option } from 'ts-option';
import { Redirect } from 'react-router';
import { sendLoginError } from 'MyRE/Actions/Auth';

interface IOwnProps { }
interface IConnectedState {
    isLoggedIn: Option<boolean>;
    loginMessageExists: boolean;
}
interface IConnectedDispatch {
    sendLoginError: (message: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    isLoggedIn: state.auth.isLoggedIn,
    loginMessageExists: state.auth.loginMessage.isDefined,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    sendLoginError: (message) => {
        dispatch(sendLoginError(message));
    }
});

class LoginPageComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch> {
    private getParameterByName(name: string) {
        let url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    public render() {
        if (this.props.isLoggedIn.isDefined && this.props.isLoggedIn.get) {
            return <Redirect to="/" />
        }

        let loginMessage = this.getParameterByName("loginError");
        if (!this.props.loginMessageExists && !!loginMessage && loginMessage.length > 0) {
            this.props.sendLoginError(loginMessage);
        }

        return (
            <Container>
                <Row>
                    <Col xs="12" md="5">
                        <Row>
                            <h3>Log In</h3>                            
                        </Row>
                        <Row>
                            <LoginForm />
                        </Row>
                    </Col>
                    <Col xs="12" md="2">Or</Col>
                    <Col xs="12" md="5">
                        <Row>
                            <h3>Register</h3>
                        </Row>
                        <Row>
                            <RegistrationForm />
                        </Row>
                    </Col>
                </Row>
            </Container>
        );
    }
}

export const LoginPage = connect(mapStateToProps, mapDispatchToProps)(LoginPageComponent);