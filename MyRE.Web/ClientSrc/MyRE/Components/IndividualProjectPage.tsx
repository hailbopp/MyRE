import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';
import { Switch, Route, RouteComponentProps } from 'react-router';
import { Container } from 'reactstrap';
import { PageHeader } from 'MyRE/Components/PageHeader';
import { ProjectListing } from 'MyRE/Api/Models';
import { List } from 'immutable';

type IOwnProps = RouteComponentProps<{ projectId: string; }>;
interface IConnectedState {
    projectMetadata: Store.Project | undefined;
}

interface IConnectedDispatch {
}

export type IIndividualProjectPageProperties = IOwnProps & IConnectedState & IConnectedDispatch;

const mapStateToProps = (state: Store.All, ownProps: IOwnProps): IConnectedState => ({
    projectMetadata: state.projects.projects.getOrElse(List([])).toArray().find(pl => ownProps.match.params.projectId === pl.projectId),
});

const mapDispatchToProps = (dispatch: Dispatch<Store.All>): IConnectedDispatch => ({
});



class IndividualProjectPageComponent extends React.PureComponent<IIndividualProjectPageProperties> {
    public render() {
        return (
            <Container>
                <PageHeader>{this.props.projectMetadata && this.props.projectMetadata.name}</PageHeader>
                <Switch>
                    { /*<Route exact path="/projects/:projectId" component={ProjectEditPage} />*/ }
                </Switch>
            </Container>
        );
    }
}

export const IndividualProjectPage =
    connect(mapStateToProps, mapDispatchToProps)(
        IndividualProjectPageComponent);
