import * as React from 'react';
import { IUserSummary } from "../store/admin-store";
import { Table } from "reactstrap";

interface IUserTable {
    users: IUserSummary[]
}

export class UserTable extends React.Component<IUserTable, undefined> {
    render() {
        let rows = this.props.users.sort((a,b)=>a.realName.localeCompare(b.realName)).map(u => (
            <tr key={u.id}>
                <td>
                    {u.realName}
                </td>
                <td>
                    {u.emailAddress}
                </td>
                <td>
                    {u.role}
                </td>
            </tr>
        ));

        return (<Table responsive>
            <thead>
                <th>
                    Name
                </th>
                <th>
                    Email
                </th>
                <th>
                    Role
                </th>
            </thead>
            <tbody>
                {rows}
            </tbody>
        </Table>)
    }
}