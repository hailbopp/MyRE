import * as React from 'react';
import { Store } from 'FreeCoRE/Models/Store';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'reactstrap';
import { LoginForm } from 'FreeCoRE/Components/LoginForm';
import { RegistrationForm } from 'FreeCoRE/Components/RegistrationForm';
import { Option } from 'ts-option';
import { Redirect } from 'react-router';

interface IOwnProps { }
interface IConnectedState {
    isLoggedIn: Option<boolean>;
}
interface IConnectedDispatch {
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    isLoggedIn: state.auth.isLoggedIn,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});

class LoginPageComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch> {
    public render() {
        if (this.props.isLoggedIn.isDefined && this.props.isLoggedIn.get) {
            return <Redirect to="/" />
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