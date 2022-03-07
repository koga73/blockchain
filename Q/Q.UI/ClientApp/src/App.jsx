import React from "react";
import {BrowserRouter, Routes, Route, Link} from "react-router-dom";

import PageHome from "./pages/Home/PageHome";
import PageUsers from "./pages/Users/PageUsers";
import PageSettings from "./pages/Settings/PageSettings";
import PageMining from "./pages/Mining/PageMining";

import "./App.scss";

function App() {
	return (
		<BrowserRouter>
			<div id="page">
				<header className="wrap">
					<h1>Queuechain</h1>
					<ul>
						<li>
							<Link to="/" title="Messages">
								<span className="hide-text">Messages</span>
								<i className="fas fa-comment"></i>
							</Link>
						</li>
						<li>
							<Link to="/users" title="Users">
								<span className="hide-text">Users</span>
								<i className="fas fa-users"></i>
							</Link>
						</li>
						<li>
							<Link to="/settings" title="Settings">
								<span className="hide-text">Settings</span>
								<i className="fas fa-cog"></i>
							</Link>
						</li>
						<li>
							<Link to="/mining" title="Mining">
								<span className="hide-text">Mining</span>
								<i className="fas fa-coins"></i>
							</Link>
						</li>
					</ul>

					{/*
					<div className="select-wrap">
						<label>Account:</label>
						<select>
							<option disabled>Select an account</option>
							<option value="new">Create new account</option>
							<option value="manage">Manage accounts</option>
						</select>
					</div>
					*/}
				</header>
				{/*<main className={["wrap", section ? `section-${section}` : null].join(" ").trim().replace(/\s+/g, " ")}>*/}
				<main className="wrap">
					<Routes>
						<Route exact path="/" element={<PageHome />} />
						<Route exact path="/users" element={<PageUsers />} />
						<Route exact path="/settings" element={<PageSettings />} />
						<Route exact path="/mining" element={<PageMining />} />
					</Routes>
				</main>
				<footer className="wrap">
					<p>&copy; Copyright 2022 Savino Systems LLC</p>
					<p>
						<Link to="mailto:info@savino.systems">info@savino.systems</Link>
					</p>
				</footer>
			</div>
		</BrowserRouter>
	);
}
export default App;
