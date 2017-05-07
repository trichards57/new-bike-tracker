import { UserStore } from "./user-store";
import { AdminStore } from "./admin-store";
import { observable, action } from 'mobx';

export class AppStore {
    @observable user: UserStore = new UserStore(this);
    @observable error: string = "";
    @observable admin: AdminStore = new AdminStore(this);

    @action showError = (msg: string) => {
        this.error = msg;
    }

    @action clearError = () => {
        this.error = "";
    }
}