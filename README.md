# HTML Zoning and Main Content Detection Tool

This repository contains the code and data sets for the paper "Main Content Detection in HTML Journal Articles" by A R Rae et al. A "Visualizer" GUI is provided to run the algorithms described in the paper.

### Prerequisites

* Windows
* .NET 4.6.2 or greater
* Microsoft Visual Studio 2015 or greater (or alternative compatible compiler/IDE)

### How to run the main content detection algorithms

1) Build HtmlZoning.sln
2) Run the Visualizer GUI
3) Enter a web page URL into the text box directly below the menu strip, press Enter, and wait for the web page to render.
4) From the menu click Actions > Do Zoning. When the zoning has finished the zone tree will be displayed in the panel on the right.
5) Finally, click Actions > Label Article Content to run the main content detection algorithm of interest. When the algorithm has finished, a check box labeled "ArticleContent" will appear in the top right panel, check this to highlight the zones that have been detected as main content.

### Datasets

The data sets are in the .\data folder. test-set-1-pmids.csv contains the PubMed ids (PMIDs) of the full test set (359 articles) and test-set-2-pmids.csv contains the PMIDs for the 158 online journal articles that contained the HTML article/main tag.