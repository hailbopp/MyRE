import * as Auth from 'MyRE/Actions/Auth';
import * as Nav from 'MyRE/Actions/Nav';
import * as Instances from 'MyRE/Actions/Instances';
import * as Projects from 'MyRE/Actions/Projects';

type UndefinedAction = { type: '' };

export type AppAction =
    | Auth.AuthAction

    | Nav.OpenNavPaneAction
    | Nav.CloseNavPaneAction

    | Instances.RequestUserInstanceListApiAction
    | Instances.ReceivedUserInstanceListApiAction

    | Projects.ProjectAction

    | UndefinedAction