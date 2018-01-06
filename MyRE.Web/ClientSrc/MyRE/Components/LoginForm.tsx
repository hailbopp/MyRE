import * as React from 'react';
import { Store } from 'MyRE/Models/Store';
import { Dispatch, connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { InputField } from 'MyRE/Components/InputField';
import { Button, Row, Container, Form, FormGroup, Label, Col, Input, Alert } from 'reactstrap';
import { attemptLogin } from 'MyRE/Actions/Auth';
import { Option } from 'ts-option';
import { AlertRow } from 'MyRE/Components/AlertRow';

interface IOwnProps {
}

interface IOwnState {
    email: string;
    password: string;
}
interface IConnectedState {
    loginMessage: Option<Store.AlertMessage>;
}
interface IConnectedDispatch {
    attemptLogIn: (email: string, password: string) => void;
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    loginMessage: state.auth.loginMessage,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    attemptLogIn: (email, password) => {
        dispatch(attemptLogin({ email, password }));
    }
});

type LoginFormProps = IOwnProps & IConnectedDispatch & IConnectedState;

class LoginFormComponent extends React.Component<LoginFormProps, IOwnState> {
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
        this.props.attemptLogIn(this.state.email, this.state.password);
    }

    public render() {
        return (
            <Container>
                <AlertRow message={this.props.loginMessage} />
                <FormGroup row>
                    <Label for="login-email" xs="12" md="3">Email</Label>
                    <Col xs="12" md="9">
                        <Input type="email" name="login-email"
                            value={this.state.email}
                            onChange={this.changeEmail} />
                    </Col>                    
                </FormGroup>
                <FormGroup row>
                    <Label for="login-password" xs="12" md="3">Password</Label>
                    <Col xs="12" md="9">
                        <Input type="password" name="login-password"
                            value={this.state.password}
                            onChange={this.changePassword} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Button onClick={this.submit}>Log In</Button>
                </FormGroup>
            </Container>
        );
    }
}

export const LoginForm = connect(mapStateToProps, mapDispatchToProps)(LoginFormComponent);
