Feature: Graph manager can connect nodes
	In order to establish a graph
	As a user
	I want to be able to connect nodes

Scenario Outline: Node connections are Reflexive
	Given I have a <GraphManager> with <n> nodes
	 Then number of nodes equals <n>
	  And each node should be connected to itself
	Examples:
	| GraphManager                   | n  |
	| QuickFindGraphManager          | 10 |
	| QuickWeigthedUnionGraphManager | 10 |

Scenario Outline: Node connections are Symmetric
	Given I have a <GraphManager> with <n> nodes
	 When I connect node <a> with node <b>
	 Then node <a> should be connected to node <b>
	  And  node <b> should be connected to node <a>
	Examples:
	| GraphManager                   | n  | a | b |
	| QuickFindGraphManager          | 10 | 5 | 6 |
	| QuickWeigthedUnionGraphManager | 10 | 1 | 9 |

Scenario Outline: Node connections are Transitive
	Given I have a <GraphManager> with <n> nodes
	 When I connect node <a> with node <b>
	 When I connect node <b> with node <c>
	 Then node <a> should be connected to node <c>
	  And node <c> should be connected to node <a>
	Examples:
	| GraphManager                   | n  | a | b | c |
	| QuickFindGraphManager          | 10 | 2 | 5 | 9 |
	| QuickWeigthedUnionGraphManager | 10 | 1 | 2 | 7 |