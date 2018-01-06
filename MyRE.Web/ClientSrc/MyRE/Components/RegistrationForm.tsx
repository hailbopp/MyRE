import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { InputField } from 'MyRE/Components/InputField';
import { Button, Input, Col, Label, FormGroup, Container, Row, Alert } from 'reactstrap';
import { attemptRegistration } from 'MyRE/Actions/Auth';
import { Option } from 'ts-option';
import { AlertRow } from 'MyRE/Components/AlertRow';

interface IOwnProps {
}

interface IOwnState {
    email: string;
    password: string;
}
interface IConnectedState {
    registrationMessage: Option<Store.AlertMessage>;
}
interface IConnectedDispatch {
    attemptRegistration: (email: string, password: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    registrationMessage: state.auth.registrationMessage,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    attemptRegistration: (email, password) => {
        dispatch(attemptRegistration({ email, password }));
    }
});

type RegistrationFormProps = IOwnProps & IConnectedDispatch & IConnectedState;

class RegistrationFormComponent extends React.Component<RegistrationFormProps, IOwnState> {
    constructor(props: any) {
        super(props);
        this.state = {
            email: '',
            password: '',
        };
    }

    private changeEmail: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.setState({
            email: e.target.value,
            password: this.state.password
        });
    }

    private changePassword: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        this.setState({
            email: this.state.email,
            password: e.target.value
        });
    }

    private submit = () => {
        this.props.attemptRegistration(this.state.email, this.state.password);
    }

    public render() {
        return (
            <Container>

                <AlertRow message={this.props.registrationMessage} />
                <FormGroup row>
                    <Label for="register-email" xs="12" md="3">Email</Label>
                    <Col xs="12" md="9">
                        <Input type="email" name="register-email"
                            value={this.state.email}
                            onChange={this.changeEmail} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Label for="register-password" xs="12" md="3">Password</Label>
                    <Col xs="12" md="9">
                        <Input type="password" name="register-password"
                            value={this.state.password}
                            onChange={this.changePassword} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Button type="submit" onClick={this.submit}>Register</Button>
                </FormGroup>
            </Container>
        );
    }
}

export const RegistrationForm = connect(mapStateToProps, mapDispatchToProps)(RegistrationFormComponent);
