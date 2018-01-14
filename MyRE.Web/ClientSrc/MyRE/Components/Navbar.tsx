import * as Radium from 'radium';
import * as React from 'react';
import { Provider, connect, Dispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { APP_NAME } from 'MyRE';
import { Store } from 'MyRE/Models/Store';
import { Navbar as BSNavbar, NavbarBrand, NavbarToggler, Nav, NavItem, Collapse, NavLink } from 'reactstrap';
import { openNavPane, closeNavPane } from 'MyRE/Actions/Nav';
import { initiateLogout } from 'MyRE/Actions/Auth';
import { Option } from 'ts-option';

interface IOwnProps { }
interface IConnectedState {
    isNavOpen: boolean;
    isLoggedIn: Option<boolean>;
}
interface IConnectedDispatch {
    openNav: () => void,
    collapseNav: () => void,
    logOut: () => void,
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    isNavOpen: state.nav.navPaneOpen,
    isLoggedIn: state.auth.isLoggedIn,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    openNav: () => dispatch(openNavPane()),
    collapseNav: () => dispatch(closeNavPane()),
    logOut: () => dispatch(initiateLogout()),
});

class NavbarComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch> {
    private toggleCollapse = () => {
        if (this.props.isNavOpen) {
            this.props.collapseNav();
        } else {
            this.props.openNav();
        }
    }

    private logOut = this.props.logOut;

    public render() {
        return (
            <BSNavbar color="faded" light expand="md">
                <Link className="navbar-brand" to="/">{APP_NAME}</Link>
                <NavbarToggler onClick={this.toggleCollapse} />
                <Collapse isOpen={this.props.isNavOpen} navbar>
                    {
                        this.props.isLoggedIn.isDefined && this.props.isLoggedIn.get &&
                            <Nav navbar className="ml-auto">
                                <NavItem>
                                    <Link className="nav-link" to="/projects">Projects</Link>
                                </NavItem>
                                <NavItem>
                                    <NavLink href="#" onClick={this.logOut}>Log Out</NavLink>
                                </NavItem>
                            </Nav>
                    } 
                </Collapse>
            </BSNavbar>
        );
    }
}

export const Navbar =
    connect(mapStateToProps, mapDispatchToProps)(
        NavbarComponent);