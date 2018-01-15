import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { InputField } from 'MyRE/Components/InputField';
import { Button, Row, Container, Form, FormGroup, Label, Col, Input, Alert } from 'reactstrap';
import { attemptLogin, updateLoginForm } from 'MyRE/Actions/Auth';
import { Option } from 'ts-option';
import { AlertRow } from 'MyRE/Components/AlertRow';

interface IOwnProps {
}

interface IOwnState {
}
interface IConnectedState {
    loginMessage: Option<Store.AlertMessage>;
    loginForm: Store.EmailPasswordForm;
}
interface IConnectedDispatch {
    attemptLogIn: (email: string, password: string) => void;
    updateFieldValues: (email: string, password: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    loginMessage: state.auth.loginMessage,
    loginForm: state.auth.loginForm,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    attemptLogIn: (email, password) => {
        dispatch(attemptLogin({ email, password }));
    },
    updateFieldValues: (email, password) => {
        dispatch(updateLoginForm({ emailValue: email, passwordValue: password }));
    }
});

type LoginFormProps = IOwnProps & IConnectedDispatch & IConnectedState;

class LoginFormComponent extends React.Component<LoginFormProps> {
    private submit = () => {
        this.props.attemptLogIn(this.props.loginForm.emailValue, this.props.loginForm.passwordValue);
    }

    private changeEmail: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.updateFieldValues(e.target.value, this.props.loginForm.passwordValue);
    }

    private changePassword: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.updateFieldValues(this.props.loginForm.emailValue, e.target.value);
    }

    public render() {
        return (
            <Container>
                <AlertRow message={this.props.loginMessage} />
                <FormGroup row>
                    <Label for="login-email" xs="12" md="3">Email</Label>
                    <Col xs="12" md="9">
                        <Input type="email" name="login-email"
                            value={this.props.loginForm.emailValue}
                            onChange={this.changeEmail} />
                    </Col>                    
                </FormGroup>
                <FormGroup row>
                    <Label for="login-password" xs="12" md="3">Password</Label>
                    <Col xs="12" md="9">
                        <Input type="password" name="login-password"
                            value={this.props.loginForm.passwordValue}
                            onChange={this.changePassword} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Button color="primary" onClick={this.submit}>Log In</Button>
                </FormGroup>
            </Container>
        );
    }
}

export const LoginForm = connect(mapStateToProps, mapDispatchToProps)(LoginFormComponent);
