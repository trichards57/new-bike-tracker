import { observable, action } from 'mobx';
import { AppStore } from "./app-store";

export interface IUserSummary {
    realName: string;
    emailAddress: string;
    id: string;
    role: string;
}

export class AdminStore {
    constructor(parentStore: AppStore) {
        this._appStore = parentStore;
    }

    private _appStore: AppStore;

    @observable loadingUsers: boolean = true;
    @observable users: IUserSummary[];

    @action loadUsers = async () => {
        this.loadingUsers = true;
        try {
            let result = await fetch("/api/user", {
                method: "GET",
                credentials: "same-origin"
            });

            if (result.ok) {
                this.users = await result.json() as IUserSummary[];
            }

            this.loadingUsers = false;
        }
        catch (err) {
            this._appStore.showError("Could not contact server.  Are you online?");
        }
    };
}