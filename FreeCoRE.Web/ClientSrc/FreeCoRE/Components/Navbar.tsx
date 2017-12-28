import * as Radium from 'radium';
import * as React from 'react';
import { Provider, connect, Dispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { APP_NAME } from 'FreeCoRE';
import { Store } from 'FreeCoRE/Models/Store';
import { Navbar as BSNavbar, NavbarBrand, NavbarToggler, Nav, NavItem, Collapse, NavLink } from 'reactstrap';
import { openNavPane, closeNavPane } from 'FreeCoRE/Actions/Nav';

interface IOwnProps { }
interface IConnectedState {
    isNavOpen: boolean;
}
interface IConnectedDispatch {
    openNav: () => void,
    collapseNav: () => void,
}

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    isNavOpen: state.nav.navPaneOpen,
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
    openNav: () => dispatch(openNavPane()),
    collapseNav: () => dispatch(closeNavPane()),
});

class NavbarComponent extends React.Component<IOwnProps & IConnectedState & IConnectedDispatch> {
    private toggleCollapse = () => {
        if (this.props.isNavOpen) {
            this.props.collapseNav();
        } else {
            this.props.openNav();
        }
    }

    public render() {
        return (
            <BSNavbar color="faded" light expand="md">
                <Link className="navbar-brand" to="/">{APP_NAME}</Link>
                <NavbarToggler onClick={this.toggleCollapse} />
                <Collapse isOpen={this.props.isNavOpen} navbar>
                    <Nav navbar className="ml-auto">
                        <NavItem>
                            <Link className="nav-link" to="/">Scripts</Link>
                        </NavItem>
                    </Nav>
                </Collapse>
            </BSNavbar>
        );
    }
}

export const Navbar =
    connect(mapStateToProps, mapDispatchToProps)(
        NavbarComponent);