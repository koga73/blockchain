import React, {useEffect, useState} from "react";

import Api from "../../services/Api";

import "./PageMining.scss";

const INTERVAL_MINING = 500; //Milliseconds

function PageMining() {
	const [stateUsers, setStateUsers] = useState(null);
	const [stateUser, setStateUser] = useState("none");
	const [stateIsMining, setStateIsMining] = useState(false);
	const [stateLog, setStateLog] = useState([]);

	useEffect(function componentDidMount() {
		//Get users
		Api.getUsers()
			.then((users) => setStateUsers(users.data))
			.catch((err) => console.error(err));

		//Check mining
		getIsMining();
		const interval = setInterval(getIsMining, INTERVAL_MINING);

		return function componentDidUnmount() {
			clearInterval(interval);
		};
	}, []);

	function getIsMining() {
		Api.isMining()
			.then((response) => {
				const isMining = response.data === true;
				setStateIsMining(isMining);
				setStateLog((prevLog) => [...prevLog, isMining ? "Mining..." : "Not Mining"]);
			})
			.catch((err) => console.error(err));
	}

	function handler_start_click() {
		//TODO: Remove hard-coded values
		Api.startMining(`${stateUser}${new Date().getTime()}`, stateUser)
			.then((response) => {
				setStateLog([...stateLog, "Started Mining!"]);
			})
			.catch((err) => console.error(err));
	}

	function handler_stop_click() {
		Api.startMining()
			.then((response) => {
				setStateLog([...stateLog, "Stopped Mining"]);
			})
			.catch((err) => console.error(err));
	}

	return (
		<React.Fragment>
			<section id="mining">
				<h2>Mining</h2>

				{stateUsers && (
					<div className="select-wrap">
						<label>Select a user to earn mining rewards</label>
						<select value={stateUser} onChange={(evt) => setStateUser(evt.target.value)}>
							<option value="none" disabled>
								Select a user
							</option>
							{stateUsers.map((user) => (
								<option value={user.alias} key={user.alias}>
									{user.alias}
								</option>
							))}
						</select>
					</div>
				)}

				{!stateIsMining && (
					<button onClick={handler_start_click}>
						<i className="fas fa-play"></i>
						<span>Start Mining</span>
					</button>
				)}
				{stateIsMining && (
					<button onClick={handler_stop_click}>
						<i className="fas fa-stop"></i>
						<span>Stop Mining</span>
					</button>
				)}
			</section>
			<section id="miningLog">
				<h3>Mining Log</h3>

				{stateLog.map((log, lineIndex) => (
					<p key={lineIndex}>{log}</p>
				))}
			</section>
		</React.Fragment>
	);
}
export default PageMining;
