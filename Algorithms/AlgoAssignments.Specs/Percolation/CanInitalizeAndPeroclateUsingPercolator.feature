Feature: Can Initalize And Peroclate Using Percolator
	In order to assess and time trials on graph implementations
	As a user
	I want to be able to initialize and percolate the percolator

Scenario Outline: Peroclator initializes to all full and does not percolate
	Given I have a <GraphManager> peroclator with a dimension of <n>
	  And I initialize the percolator
	 Then all squares are open
	  And Peroclator does not percolate
	Examples:
	| GraphManager                   | n |
	| QuickFindGraphManager          | 5 |
	| QuickWeigthedUnionGraphManager | 5 |

Scenario Outline: Peroclator with open adjacent squares from top to bottom percolates
	Given I have a <GraphManager> peroclator with a dimension of <n>
	  And I initialize the percolator
	 When I connect a line of squares from top to bottom
	 Then Peroclator does percolate
	Examples:
	| GraphManager                   | n |
	| QuickFindGraphManager          | 5 |
	| QuickWeigthedUnionGraphManager | 5 |

Scenario Outline: Peroclator percolates with run time
	Given I have a <GraphManager> peroclator with a dimension of <n>
	 When it percolates
	 Then run time is greater than zero
	Examples:
	| GraphManager                   | n |
	| QuickFindGraphManager          | 5 |
	| QuickWeigthedUnionGraphManager | 5 |