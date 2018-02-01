# HTML Zoning and Main Content Detection Tool

This repository contains the code and datasets for the paper "Main Content Detection in HTML Journal Articles" by A. R. Rae et al. 

## Prerequisites

* Microsoft Windows
* .NET 4.6.2 or greater
* Microsoft Visual Studio 2015 or greater (or alternative compatible compiler)

## Datasets

The datasets are in the .\data folder. test-set-1-pmids.csv contains the PubMed ids (PMIDs) of the full test set (359 articles) and test-set-2-pmids.csv contains the PMIDs of the subset of 158 online journal articles that contained the HTML \<article> or \<main> tag.

## How to run the main content detection algorithms in the Visualizer GUI

1) Run the Visualizer GUI.
2) Enter a web page URL into the text box directly below the menu strip, press Enter, and wait for the web page to render.
3) From the menu click Actions > Do Zoning. When the zoning has finished the zone tree will be displayed in the panel on the right.
4) Finally, click Actions > Label Article Content to run the main content detection algorithm of interest. When the algorithm has finished, a check box labeled "ArticleContent" will appear in the top right panel, check this to highlight the zones that have been detected as main content.

## How to evaluate the performance of the main content detection algorithms

1) Create a folder for the baseline data.
2) For each PMID in the test set lookup the publisher full text URL using PubMed (www.pubmed.gov). In the paper, the PubMed Central URL was used, instead of the publisher URL, for three PMIDs (26880728, 26817485, 27029637).
3) For each PMID create a text file containing only the URL. The text file name should be the PMID and the text file extension should be ".url".
4) Open each .url file in the Manual Labeler GUI. Click File > Load Url.
5) Wait for the web page to render and then do the HTML zoning (Click Actions > Do Zoning).
6) When the zoning has finished, a list of suggested label names will appear in the panel on the right.
7) Choose the "content" label and then select the main content zones using the mouse (hint: hold down the left mouse button and drag to create a selection rectangle).
8) Next, save the labeled zone tree to file by clicking File > Save Zone Tree. You may also save a screenshot at any time by clicking File > Save Screenshot. Saving a screenshot is useful as the web page may be modified in the future.
9) After finishing the manual labeling for all PMIDs, run the Performance Evaluation console application as follows:

    ```PerformanceEvaluation.exe \path\to\baseline\data\ baseline-label algorithm-name \path\to\results.csv```

    * argument 1: full path of folder containing the baseline data
    * argument 2: main content label used in the baseline data (default: content)
    * argument 3: name of algorithm to run (layout/articletag/maintag)
    * argument 4: full path of the results file to create

    The console application writes the computed precision, recall, and F1-score for each PMID to the specified results file in CSV format.




 