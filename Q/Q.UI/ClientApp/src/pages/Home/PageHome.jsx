import React, {useState, useEffect} from "react";
import {Link, useSearchParams} from "react-router-dom";

import BottomDrawer from "../../components/BottomDrawer";
import Api from "../../services/Api";

import "./PageHome.scss";

function PageHome() {
	const [searchParams] = useSearchParams();
	const [stateQ, setStateQ] = useState(searchParams.get("q") || "");

	const [stateUsers, setStateUsers] = useState(null);
	const [stateMessages, setStateMessages] = useState(null);

	const [stateAlias, setStateAlias] = useState("");
	const [stateInput, setStateInput] = useState("");

	useEffect(function componentDidMount() {
		Api.getUsers()
			.then((users) => setStateUsers(users.data.length ? users.data : null))
			.catch((err) => console.error(err));
		Api.searchMessages(stateQ)
			.then((response) => setStateMessages(response.data))
			.catch((err) => console.error(err));
	}, []);

	function handler_frmInput_submit(evt) {
		evt.preventDefault();

		Api.postMessage(stateAlias, stateInput)
			.then((response) => {
				alert("Your message will be posted after the next block is mined!");
			})
			.catch((err) => console.error(err));
		setStateInput("");

		return false;
	}

	return (
		<section id="home">
			<h2>Messages</h2>
			<form id="frmSearch">
				<div className="text-wrap">
					<label htmlFor="txtSearch">Search:</label>
					<input id="txtSearch" name="q" type="search" placeholder="@handle, #hashtag, keyword" value={stateQ} onChange={(evt) => setStateQ(evt.target.value)} />
				</div>
				<button type="submit">
					<span className="hide-text">Search</span>
					<i className="fas fa-search"></i>
				</button>
			</form>
			<hr />
			{stateMessages && (
				<div id="messages">
					<ol>
						{stateMessages.map((message) => (
							<li key={message.hash}>
								<span>{message.alias}</span>
								<span>{message.timestamp}</span>
								<span>{message.data}</span>
							</li>
						))}
					</ol>
				</div>
			)}
			<BottomDrawer>
				{!stateUsers && (
					<Link to="/users" className="btn">
						Register to post messages
					</Link>
				)}
				{stateUsers && (
					<form id="frmInput" onSubmit={handler_frmInput_submit}>
						<div className="select-wrap">
							<label htmlFor="sltUser">Alias:</label>
							<select id="sltUser" name="sltUser" value={stateAlias} onChange={(evt) => setStateAlias(evt.target.value)}>
								{stateUsers.map((user) => (
									<option key={user.publicKey}>{user.alias}</option>
								))}
							</select>
						</div>
						<div className="text-wrap">
							<label htmlFor="txtInput">Write a message:</label>
							<input
								id="txtInput"
								name="txtInput"
								type="text"
								placeholder="@handle it's a great day! #happylife"
								value={stateInput}
								onChange={(evt) => setStateInput(evt.target.value)}
							/>
						</div>
						<button type="submit">
							<span className="hide-text">Post</span>
							<i className="fas fa-arrow-right"></i>
						</button>
					</form>
				)}
			</BottomDrawer>
		</section>
	);
}
export default PageHome;
