import React, {useEffect, useState, useCallback} from "react";
import {Link} from "react-router-dom";

import BottomDrawer from "../../components/BottomDrawer";
import Api from "../../services/Api";

import "./PageMining.scss";

const INTERVAL_MINING = 1000; //Milliseconds

function PageMining() {
	const [stateUsers, setStateUsers] = useState(null);
	const [stateUser, setStateUser] = useState("none");
	const [stateIsMining, setStateIsMining] = useState(false);
	const [stateLog, setStateLog] = useState([]);
	const [stateInterval, setStateInterval] = useState(0);

	useEffect(function componentDidMount() {
		//Get users
		Api.getUsers()
			.then((users) => setStateUsers(users.data))
			.catch((err) => console.error(err));

		//Check mining
		getIsMining();

		return function componentDidUnmount() {
			if (stateInterval) {
				clearInterval(stateInterval);
				setStateInterval(0);
			}
		};
	}, []);

	const getIsMining = useCallback(() => {
		Api.isMining()
			.then((response) => {
				const isMining = response.data.isMining === true;
				setStateIsMining(isMining);
				setStateLog([...stateLog, isMining ? `Mining speed: ${response.data.miningSpeed}` : "Not Mining"]);
				if (isMining && !stateInterval) {
					setStateInterval(setInterval(getIsMining, INTERVAL_MINING));
				} else if (stateInterval) {
					clearInterval(stateInterval);
					setStateInterval(0);
				}
			})
			.catch((err) => console.error(err));
	}, [stateLog, stateInterval]);

	const handler_start_click = useCallback(() => {
		//TODO: Remove hard-coded values
		Api.startMining(`${stateUser}${new Date().getTime()}`, stateUser)
			.then((response) => {
				setStateLog([...stateLog, "Started Mining!"]);
				getIsMining();
			})
			.catch((err) => console.error(err));
	}, [stateLog]);

	const handler_stop_click = useCallback(() => {
		Api.startMining()
			.then((response) => {
				setStateLog([...stateLog, "Stopped Mining"]);
			})
			.catch((err) => console.error(err));
	}, [stateLog]);

	return (
		<React.Fragment>
			<section id="mining">
				<h2>Mining</h2>
				<section id="miningLog">
					<h3>Mining Log</h3>

					{stateLog.map((log, lineIndex) => (
						<p key={lineIndex}>{log}</p>
					))}
				</section>

				<BottomDrawer>
					{!stateUsers && (
						<Link to="/users" className="btn">
							Register to begin mining
						</Link>
					)}
					{stateUsers && (
						<form>
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

							{!stateIsMining && (
								<button type="button" onClick={handler_start_click} className="btn">
									<i className="fas fa-play"></i>
									<span>Start Mining</span>
								</button>
							)}
							{stateIsMining && (
								<button type="button" onClick={handler_stop_click} className="btn">
									<i className="fas fa-stop"></i>
									<span>Stop Mining</span>
								</button>
							)}
						</form>
					)}
				</BottomDrawer>
			</section>
		</React.Fragment>
	);
}
export default PageMining;
