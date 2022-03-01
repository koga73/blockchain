import React, {useState, useEffect} from "react";

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
		<React.Fragment>
			<section id="users">
				<h2>Users</h2>
				{stateUsers && (
					<table>
						<thead>
							<tr>
								<th>Alias</th>
								<th>PublicKey</th>
							</tr>
						</thead>
						<tbody>
							{stateUsers.map((user) => (
								<tr key={user.alias}>
									<td className="alias">{user.alias}</td>
									<td className="publicKey">{user.publicKey}</td>
								</tr>
							))}
						</tbody>
					</table>
				)}
			</section>
			<section id="register">
				<h3>Register</h3>
				<form onSubmit={handler_register_submit}>
					<div className="text-wrap">
						<label htmlFor="txtRegister">Alias</label>
						<input id="txtRegister" name="txtRegister" type="text" value={stateAlias} onChange={(evt) => setStateAlias(evt.target.value)} />
					</div>
					<button type="submit">Register Alias</button>
				</form>
			</section>
		</React.Fragment>
	);
}
export default PageUsers;
