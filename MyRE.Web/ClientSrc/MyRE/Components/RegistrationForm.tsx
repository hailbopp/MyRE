import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { InputField } from 'MyRE/Components/InputField';
import { Button, Input, Col, Label, FormGroup, Container, Row, Alert } from 'reactstrap';
import { attemptRegistration, updateRegistrationForm } from 'MyRE/Actions/Auth';
import { Option } from 'ts-option';
import { AlertRow } from 'MyRE/Components/AlertRow';

interface IConnectedState {
    registrationMessage: Option<Store.AlertMessage>;
    registerForm: Store.EmailPasswordForm;
}
interface IConnectedDispatch {
    attemptRegistration: (email: string, password: string) => void;
    updateFieldValues: (email: string, password: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: {}): IConnectedState => ({
    registrationMessage: state.auth.registrationMessage,
    registerForm: state.auth.registrationForm,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    attemptRegistration: (email, password) => {
        dispatch(attemptRegistration({ email, password }));
    },
    updateFieldValues: (email, password) => {
        dispatch(updateRegistrationForm({ emailValue: email, passwordValue: password }));
    }
});

type RegistrationFormProps = IConnectedDispatch & IConnectedState;

class RegistrationFormComponent extends React.Component<RegistrationFormProps> {
    constructor(props: any) {
        super(props);
        this.state = {
            email: '',
            password: '',
        };
    }

    private changeEmail: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.updateFieldValues(e.target.value, this.props.registerForm.passwordValue);
    }

    private changePassword: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.props.updateFieldValues(this.props.registerForm.emailValue, e.target.value);
    }

    private submit = () => {
        this.props.attemptRegistration(this.props.registerForm.emailValue, this.props.registerForm.passwordValue);
    }

    public render() {
        return (
            <Container>

                <AlertRow message={this.props.registrationMessage} />
                <FormGroup row>
                    <Label for="register-email" xs="12" md="3">Email</Label>
                    <Col xs="12" md="9">
                        <Input type="email" name="register-email"
                            value={this.props.registerForm.emailValue}
                            onChange={this.changeEmail} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Label for="register-password" xs="12" md="3">Password</Label>
                    <Col xs="12" md="9">
                        <Input type="password" name="register-password"
                            value={this.props.registerForm.passwordValue}
                            onChange={this.changePassword} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Button color="primary" type="submit" onClick={this.submit}>Register</Button>
                </FormGroup>
            </Container>
        );
    }
}

export const RegistrationForm = connect(mapStateToProps, mapDispatchToProps)(RegistrationFormComponent);
