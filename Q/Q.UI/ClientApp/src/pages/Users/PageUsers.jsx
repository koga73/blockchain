import React, {useState, useEffect} from "react";

import BottomDrawer from "../../components/BottomDrawer";
import Api from "../../services/Api";

import "./PageUsers.scss";

function PageUsers() {
	const [stateUsers, setStateUsers] = useState(null);
	const [stateAlias, setStateAlias] = useState("");

	useEffect(function componentDidMount() {
		Api.getUsers()
			.then((users) => setStateUsers(users.data))
			.catch((err) => console.error(err));
	}, []);

	function handler_register_submit(evt) {
		evt.preventDefault();

		//TODO: Form validation
		const alias = stateAlias;
		setStateAlias("");

		Api.registerUser(alias)
			.then((response) => {
				alert("SUCCESS! Your user will be registered after the next block is mined.");
			})
			.catch((err) => console.error(err));

		return false;
	}

	return (
		<section id="users">
			<h2>Users</h2>
			{stateUsers && (
				<table>
					<thead>
						<tr>
							<th>Alias</th>
						</tr>
					</thead>
					<tbody>
						{stateUsers.map((user) => (
							<tr key={user.alias}>
								<td className="alias" title={user.publicKey}>
									{user.alias}
								</td>
							</tr>
						))}
					</tbody>
				</table>
			)}

			<BottomDrawer>
				<form onSubmit={handler_register_submit}>
					<div className="text-wrap">
						<label htmlFor="txtRegister" className="hide-text">
							Alias
						</label>
						<input id="txtRegister" name="txtRegister" type="text" placeholder="Alias" value={stateAlias} onChange={(evt) => setStateAlias(evt.target.value)} />
					</div>
					<button type="submit" className="btn">
						Register
					</button>
				</form>
			</BottomDrawer>
		</section>
	);
}
export default PageUsers;
