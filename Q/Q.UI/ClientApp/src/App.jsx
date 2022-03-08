import React, {useState} from "react";
import {BrowserRouter, Routes, Route, Link} from "react-router-dom";

import PageHome from "./pages/Home/PageHome";
import PageUsers from "./pages/Users/PageUsers";
import PageSettings from "./pages/Settings/PageSettings";
import PageMining from "./pages/Mining/PageMining";

import "./App.scss";

function App() {
	const [stateAccount, setStateAccount] = useState("select");

	return (
		<BrowserRouter>
			<div id="page">
				<header>
					<div className="content-left">
						<h1>Queuechain</h1>
						<Link to="/" id="linkMessages">
							<span className="hide-text">Messages</span>
						</Link>
					</div>
					<div className="content-right">
						<button id="btnMenu" type="button">
							<span className="hide-text">Menu</span>
							<i className="fas fa-bars"></i>
						</button>
						<nav id="menu">
							<ul>
								<li>
									<Link to="/" title="Messages">
										<i className="fas fa-comments"></i>
										<span className="link-copy">Messages</span>
									</Link>
								</li>
								<li>
									<Link to="/users" title="Users">
										<i className="fas fa-users"></i>
										<span className="link-copy">Users</span>
									</Link>
								</li>
								<li>
									<Link to="/mining" title="Mining">
										<i className="fas fa-coins"></i>
										<span className="link-copy">Mining</span>
									</Link>
								</li>
								<li>
									<Link to="/settings" title="Settings">
										<i className="fas fa-cog"></i>
										<span className="link-copy">Settings</span>
									</Link>
								</li>
							</ul>
							<footer className="wrap">
								<p>&copy;Copyright 2022 Savino Systems LLC</p>
								<p>
									<Link to="mailto:info@savino.systems">info@savino.systems</Link>
								</p>
							</footer>
						</nav>
					</div>
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
			</div>
		</BrowserRouter>
	);
}
export default App;
