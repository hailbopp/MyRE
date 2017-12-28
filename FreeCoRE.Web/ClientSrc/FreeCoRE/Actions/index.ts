import * as Nav from 'FreeCoRE/Actions/Nav';

type UndefinedAction = { type: '' };

export type AppAction =
    | Nav.OpenNavPaneAction
    | Nav.CloseNavPaneAction

    | UndefinedAction