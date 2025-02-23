%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% pnas-new.cls, v1.2, 2016/02/28 
%
% This class file enables authors to prepare research 
% articles for submission to PNAS.
% 
% Please note that whilst this template provides a 
% preview of the typeset manuscript for submission, it 
% will not necessarily be the final publication layout.
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% This work may be distributed and/or modified under the
% conditions of the LaTeX Project Public License, either
% version 1.3 of this license or any later version.
% The latest version of this license is in
% http://www.latex-project.org/lppl.txt and
% version 1.3 or later is part of all distributions
% of LaTeX version 2005/12/01 or later.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%
% For use with latex+dvipdfm, pdflatex, xelatex & lualatex 
% For compiling with plain latex, please use latex+dvipdfm 
% to produce the PDF, not dvis -> ps -> pdf nor dvipdf
% 
\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{pnas-new}[28/02/2015, v1.2]
\AtEndOfClass{\RequirePackage{microtype}}
% Option for line numbers
\newif\if@pnaslineno
\DeclareOption{lineno}{\@pnaslinenotrue}
\DeclareOption*{\PassOptionsToClass{\CurrentOption}{extarticle}}
\ProcessOptions*
\LoadClass{extarticle}

%% Fonts and language
\RequirePackage[utf8]{inputenc}
\RequirePackage{amsmath,amsfonts,amssymb}
\RequirePackage{lmodern}
\RequirePackage[scaled]{helvet}
\RequirePackage[T1]{fontenc}
\RequirePackage{lettrine} % For dropped capitals

%% For the Significance Statement & footnote on the first page
\RequirePackage{afterpage}
\RequirePackage{ifpdf,ifxetex}
\ifpdf\else
  \ifxetex\else
    \def\pgfsysdriver{pgfsys-dvipdfm.def}
    \pdfpagewidth=\paperwidth
    \pdfpageheight=\paperheight
\fi\fi
\RequirePackage{xcolor}
\RequirePackage{tikz}
\RequirePackage[framemethod=tikz]{mdframed}

%% For single column equations and balancing the columns on final page
\RequirePackage{widetext}

%% Hyperlinking
\RequirePackage[colorlinks=true, allcolors=blue]{hyperref}

%% Set up main title page fonts 
\newcommand{\headerfont}{\normalfont\sffamily\fontsize{7}{9} \selectfont}
\newcommand{\footerfont}{\normalfont\sffamily\fontsize{7}{9} \selectfont}
%\newcommand{\titlefont}{\fontfamily{lmss}\bfseries\fontsize{22pt}{24pt}\selectfont}
\newcommand{\titlefont}{\normalfont\sffamily\bfseries\fontsize{22pt}{24pt}\selectfont}
%\newcommand{\dropcapfont}{\fontfamily{lmss}\bfseries\fontsize{26pt}{28pt}\selectfont}
\newcommand{\dropcapfont}{\normalfont\sffamily\bfseries\fontsize{26pt}{28pt}\selectfont}
\newcommand{\datesfont}{\normalfont\sffamily\fontsize{7}{8}\selectfont}
\newcommand{\absfont}{\normalfont\sffamily\bfseries\fontsize{8}{11}\selectfont}
\newcommand{\keywordsfont}{\normalfont\sffamily\fontsize{7}{10}\selectfont}
\newcommand{\copyrightfont}{\normalfont\rmfamily\fontsize{6}{8}\selectfont}

%% Set URL link color & font
\renewcommand\UrlFont{\color{black}\sffamily}

%% Author and affiliation
\RequirePackage{authblk}
\setlength{\affilsep}{8.5pt} % 16.5pts between base of author line and base of affil line
\renewcommand\Authfont{\color{color0}\normalfont\sffamily\bfseries\fontsize{9}{11}\selectfont}
\renewcommand\Affilfont{\color{color0}\normalfont\sffamily\fontsize{7}{8}\selectfont}
\makeatletter
\renewcommand\AB@affilsepx{; \protect\Affilfont}
\makeatother
\renewcommand\Authands{, and }
\renewcommand{\Authand}{ and }

%% Choose template type
\newcommand*{\templatetype}[1]{%
  \RequirePackage{#1}}

%% Options for element switching
\RequirePackage{xifthen}
\newboolean{shortarticle}
\newboolean{singlecolumn}

%% For numbering just one line of an equation
\newcommand\numberthis{\addtocounter{equation}{1}\tag{\theequation}}

%% Watermark 
%\usepackage[printwatermark]{xwatermark}
%\newboolean{displaywatermark}
%\setboolean{displaywatermark}{false} % Set to false to remove the watermark
%\ifthenelse{\boolean{displaywatermark}}{%
%\newwatermark[allpages,color=gray!20,angle=45,scale=3,xpos=0,ypos=0]{DRAFT}}{}

%% Copyright statement (not used)
\newboolean{displaycopyright}
\setboolean{displaycopyright}{false} % Confirmed as not required
\RequirePackage{textcomp} % For copyright symbol styling
\newcommand{\copyrightstatement}{\, \textcopyright\, 2015 by The National Academy of Sciences of the USA}

%% Graphics, tables and other formatting
\RequirePackage{graphicx,xcolor}
\RequirePackage{colortbl}
\RequirePackage{booktabs}
\RequirePackage{algorithm}
\RequirePackage[noend]{algpseudocode}
\RequirePackage{changepage}
\RequirePackage[asymmetric,%
				a4paper,includeheadfoot,%
				%layoutsize={8.125in,10.875in},%
                %layouthoffset=0.1875in,%
                %layoutvoffset=0.0625in,%
                left=2.25in+38.5pt,%
                right=43pt,%
                top=43pt,% 10pt provided by headsep
                bottom=32pt,%
                headheight=0pt,% No Header
                headsep=10pt,%
                footskip=25pt]{geometry}
\RequirePackage[labelfont={bf,sf},%
                labelsep=period,%
                justification=RaggedRight,singlelinecheck=false]{caption}
\RequirePackage{multicol}
\setlength{\columnsep}{13.5pt} % Distance between the two columns of text
\setlength{\parindent}{12pt} % Paragraph indent
\RequirePackage{ragged2e}
\setlength{\RaggedRightParindent}{\parindent}

%% Set document color scheme
\definecolor{black50}{gray}{0.5} % 50% black for hrules
\definecolor{color0}{RGB}{0,0,0} % Base
\definecolor{color1}{RGB}{59,90,198} % author email, doi
\definecolor{color2}{RGB}{16,131,16} %
% For sig statement box
\definecolor{pnasbluetext}{RGB}{0,101,165} %
\definecolor{pnasblueback}{RGB}{205,217,235} %
%\definecolor{pnasbluetext}{RGB}{0,115,209} % Not used
%\definecolor{pnasblueback}{RGB}{210,230,247} % Not used
\definecolor{P1797}{cmyk}{0,0.94,0.94,0.06}

%% Bibliography 
% \RequirePackage[super,comma,sort&compress]{natbib}
\RequirePackage[numbers,sort&compress]{natbib}
\setlength{\bibsep}{0.0pt}
\bibliographystyle{pnas2011}
\renewcommand{\bibsection}{} % Remove header
\renewcommand\bibfont{\normalfont\sffamily\fontsize{6}{8}\selectfont} % set font to be sans serif

\makeatletter 
\renewcommand\@biblabel[1]{ #1.} % Remove brackets from label
\def\tagform@#1{\maketag@@@{\bfseries(\ignorespaces#1\unskip\@@italiccorr)}}
\renewcommand{\eqref}[1]{\textup{{\normalfont (\ref{#1}}\normalfont)}}
\makeatother

%% Figure caption style
\DeclareCaptionFormat{pnasformat}{\normalfont\sffamily\fontsize{7}{9}\selectfont#1#2#3}
\captionsetup{format=pnasformat}

%% Table style
\RequirePackage{etoolbox}
\captionsetup[table]{labelfont+={small},textfont+={small,sf,bf},skip=10pt,position=above}
% booktabs provide nice spacing, but rule widths and distances need fixing
\setlength{\heavyrulewidth}{0.5pt}
\setlength{\lightrulewidth}{0.5pt}
\setlength{\aboverulesep}{1.5pt}
\setlength{\belowrulesep}{1.5pt}
\setlength{\belowbottomsep}{10pt}
\AtBeginEnvironment{tabular}{
\sffamily\fontsize{7.5}{10}\selectfont
}
\newcommand{\addtabletext}[1]{{\setlength{\leftskip}{9pt}\fontsize{7}{9}\selectfont#1}}

%% Equation numbering - use square brackets
\makeatletter
\renewcommand\tagform@[1]{\maketag@@@ {[\ignorespaces #1\unskip \@@italiccorr ]}}
\makeatother

%% Headers and footers
\RequirePackage{fancyhdr}  % custom headers/footers
\RequirePackage{lastpage}  % Number of pages in the document
\pagestyle{fancy}          % Enables the custom headers/footers

%% For the line numbers overlay
\def\leftlinenos{%
  \pgfmathtruncatemacro{\leftstartlineno}{2*(\thepage - 1)*62 + 1}%
  \pgfmathtruncatemacro{\leftendlineno}{(2*(\thepage - 1) + 1)*62}%
  \foreach \x in {\leftstartlineno,...,\leftendlineno}{\noindent\x\\}%
}
\def\rightlinenos{%
  \pgfmathtruncatemacro{\rightstartlineno}{(2*(\thepage - 1) + 1)*62 + 1}%
  \pgfmathtruncatemacro{\rightendlineno}{(2*\thepage)*62}%
  \foreach \x in {\rightstartlineno,...,\rightendlineno}{\noindent\x\\}%
}

\definecolor{P1797}{cmyk}{0,0.94,0.94,0.06}

\makeatletter
\fancypagestyle{firststyle}{
%   \fancyfoot[R]{\footerfont \teza@program\hspace{7pt}|\hspace{7pt}\textbf{FRI}teza\hspace{7pt}|\hspace{7pt}vol. \teza@vol\hspace{7pt}|\hspace{7pt}no. \teza@no\hspace{7pt}|\hspace{7pt}\textbf{\thepage\textendash\pageref{LastPage}}}
   \fancyfoot[R]{\footerfont \teza@program\hspace{7pt}|\hspace{7pt}\color{P1797}\textbf{FRI}\color{color0}teza\hspace{7pt}|\hspace{7pt}\teza@vol\hspace{7pt}|\hspace{7pt}\teza@no\hspace{7pt}|\hspace{7pt}\textbf{\thepage\textendash\pageref{LastPage}}}
   \fancyfoot[L]{\hspace*{-2.25in}\footerfont\@ifundefined{@doi}{}{\@doi}}
}
\makeatother

% Headers
\fancyhead[LE,RO]{}
\fancyhead[LO,RE]{}
% Footers
\lfoot{}%
\cfoot{}%
\rfoot{}%
\makeatletter
\fancyfoot[LE]{\hspace*{-2.25in}\footerfont\textbf{\thepage}\hspace{7pt}|\hspace{7pt}\@ifundefined{@doi}{}{\@doi}}
%\fancyfoot[RO]{\footerfont \teza@program\hspace{7pt}|\hspace{7pt}\textbf{FRI}teza\hspace{7pt}|\hspace{7pt}vol. \teza@vol\hspace{7pt}|\hspace{7pt}no. \teza@no\hspace{7pt}|\hspace{7pt}\textbf{\thepage}}
\fancyfoot[RO]{\footerfont \teza@program\hspace{7pt}|\hspace{7pt}\color{P1797}\textbf{FRI}\color{color0}teza\hspace{7pt}|\hspace{7pt}\teza@vol\hspace{7pt}|\hspace{7pt}\teza@no\hspace{7pt}|\hspace{7pt}\textbf{\thepage}}
\fancyfoot[RE,LO]{\hspace*{-2.25in}\footerfont\@ifundefined{@leadauthor}{}{\@leadauthor}\ifnum \value{authors} > 1\hspace{5pt}\emph{\@etal}\fi}

% Use footer routine for line numbers
\AtBeginDocument{
  \RaggedRight
  \if@pnaslineno
    \ifthenelse{\boolean{singlecolumn}}{
      % use lineno package if singlecolumn
      \RequirePackage{lineno}
      \linenumbers
    }{% use tikz if twocolumn
    \fancyfoot[C]{\begin{tikzpicture}[remember picture,overlay]
    \node at([xshift=1.5em,yshift=\dimexpr -0.0625in-53pt] current page.north west)[anchor=north west,text width=3em,font=\rmfamily,align=right] {\leftlinenos};%
    \node at([xshift=-1.5em,yshift=\dimexpr -0.0625in-53pt] current page.north east)[anchor=north east,text width=3em,font=\rmfamily,align=left] {\rightlinenos};%
    \end{tikzpicture}}
    }
  \fi
}
%\makeatother

\renewcommand{\headrulewidth}{0pt}% % No header rule
\renewcommand{\footrulewidth}{0pt}% % No footer rule

%% Section/subsection/paragraph set-up
\RequirePackage[explicit]{titlesec}
\renewcommand{\thesubsection}{\Alph{subsection}}

\titleformat{\section}
  {\large\sffamily\bfseries}
  {\thesection.}
  {0.5em}
  {#1}
  []
\titleformat{name=\section,numberless}
  {\large\sffamily\bfseries}
  {}
  {0em}
  {#1}
  []  
\titleformat{\subsection}[runin]
  {\sffamily\bfseries}
  {\thesubsection.}
  {0.5em}
  {#1. }
  []
\titleformat{\subsubsection}[runin]
  {\sffamily\small\bfseries\itshape}
  {\thesubsubsection.}
  {0.5em}
  {#1. }
  []    
\titleformat{\paragraph}[runin]
  {\sffamily\small\bfseries}
  {}
  {0em}
  {#1} 
\titlespacing*{\section}{0pc}{3ex \@plus4pt \@minus3pt}{5pt}
\titlespacing*{\subsection}{0pc}{2.5ex \@plus3pt \@minus2pt}{2pt}
\titlespacing*{\subsubsection}{0pc}{2ex \@plus2.5pt \@minus1.5pt}{2pt}
\titlespacing*{\paragraph}{0pc}{1.5ex \@plus2pt \@minus1pt}{12pt}

%% Article meta data additional fields
\newcommand{\program}[1]{\def\teza@program{#1}}
\newcommand{\vol}[1]{\def\teza@vol{#1}}
\newcommand{\no}[1]{\def\teza@no{#1}}
\newcommand{\fraca}[1]{\def\@fraca{#1}\doi{frača: \href{http://fraca.si/\@fraca}{\color{black}\@fraca}}}
\newcommand{\additionalelement}[1]{\def\@additionalelement{#1}}
\newcommand{\dates}[1]{\def\@dates{#1}}
\newcommand{\doi}[1]{\def\@doi{#1}}
\newcommand{\leadauthor}[1]{\def\@leadauthor{#1}}
\newcommand{\etal}[1]{\def\@etal{#1}}
\newcommand{\keywords}[1]{\def\@keywords{#1}}
\newcommand{\authorcontributions}[1]{\def\@authorcontributions{#1}}
\newcommand{\authordeclaration}[1]{\def\@authordeclaration{#1}}
\newcommand{\equalauthors}[1]{\def\@equalauthors{#1}}
\newcommand{\correspondingauthor}[1]{\def\@correspondingauthor{#1}}
\newcommand{\significancestatement}[3]{\def\@significancestatement{\footnotesize{\bfseries #1}\par\vspace{1mm}#2\par\vspace{1mm}#3\par}}
\newcommand{\matmethods}[2]{\def\mathmethods@name{#1}\def\@matmethods{#2}}
\newcommand{\acknow}[1]{\def\@acknow{#1}}

%% default english et al.
\etal{et al.}

%% Dropped capital for first letter of main text
\newcommand{\dropcap}[1]{\lettrine[lines=2,lraise=0.05,findent=0.1em, nindent=0em]{{\dropcapfont{#1}}}{}}

%% Abstract formatting
\def\xabstract{abstract}
\long\def\abstract#1\end#2{\def\two{#2}\ifx\two\xabstract 
\long\gdef\theabstract{\ignorespaces#1}
\def\go{\end{abstract}}\else
\typeout{^^J^^J PLEASE DO NOT USE ANY \string\begin\space \string\end^^J
COMMANDS WITHIN ABSTRACT^^J^^J}#1\end{#2}
\gdef\theabstract{\vskip12pt BADLY FORMED ABSTRACT: PLEASE DO
NOT USE {\tt\string\begin...\string\end} COMMANDS WITHIN
THE ABSTRACT\vskip12pt}\let\go\relax\fi
\go}

% Define an environment with abstract content and styling
\newcommand{\abscontent}{
\noindent
{%
\parbox{\dimexpr\linewidth}{%
    \vskip3pt%
	\absfont \theabstract
}%
}%
\vskip10pt%
\noindent
{\parbox{\dimexpr\linewidth}{%
{
 \keywordsfont \@ifundefined{@keywords}{}{\@keywords}}%
}}%
\vskip12pt%
}

% Option to format abstract differently for certain layouts (not used)
\newcommand{\abscontentformatted}{
\abscontent
}

%% Manual adjustment to line up main content with line numbers
\newlength\pnas@vertadjust
\newcommand\verticaladjustment[1]{\setlength{\pnas@vertadjust}{#1}}

%% Custom title page 
\renewcommand{\@maketitle}{%
{%
\ifthenelse{\boolean{shortarticle}}
{\ifthenelse{\boolean{singlecolumn}}{}{%\begin{adjustwidth}{-2.25in}{0in}
{\raggedright\baselineskip= 24pt\titlefont \@title\par}%
\vskip10pt% 21pts between base of title and base of author line
{\raggedright \sffamily\bfseries \@author\par}
%{\raggedright \sffamily\bfseries Oskar Korošec* in Iztok Lebar Bajec\textsuperscript{a,1}\par}
%\vskip6pt% 21pts between base of title and base of author line
%{\raggedright \footnotesize\sffamily Laboratorij za računalniške strukture in sisteme\par}
%{\raggedright \footnotesize\sffamily \textsuperscript{1}Laboratorij za računalniške strukture in sisteme\par}
%\vskip8pt% 16pts between base of affiliations and base of dates line 
%{\raggedright \datesfont \@ifundefined{@dates}{}{\@dates}\par}
\vskip12pt%
%\end{adjustwidth}
\noindent\begin{tikzpicture}[overlay, remember picture]
\node[anchor=north west] at (-2.25in,0pt) {%
	\begin{minipage}{2in}
	\@ifundefined{@dates}{}{{\sffamily\footnotesize{\@dates}}\par
	\vskip6pt}
	{\sffamily\footnotesize{Iztok Lebar Bajec | associate professor | mentor}}\par
	%{\sffamily\footnotesize{\textbf{\textsuperscript{a}Mentor:} izr. prof. dr. Iztok Lebar Bajec}}\par
	%{\sffamily\footnotesize{\textsuperscript{b}\textbf{Somentor:} as. Jure Demšar}}\par 
	\vskip24pt
	%{\sffamily\footnotesize{\textbf{Copyright:} \textcopyright\,\the\year\ Rezultati diplomskega dela so intelektualna lastnina avtorja ter Fakultete za računalništvo in informatiko Univerze v Ljubljani. Za objavljanje ali izkoriščanje rezultatov diplomskega dela je potrebno pisno soglasje avtorja, Fakultete za računalništvo in informatiko ter mentorja.}}\par 
	\vskip24pt
	%{\sffamily\footnotesize{\textbf{Citiranje:} Korošec O (2016) \@title. frača: FRIteza/201516.130.}}\par 
	%{\sffamily\footnotesize{\textsuperscript{a}Mentor | izredni profesor}}\par
	%{\sffamily\footnotesize{\textsuperscript{b}Somentor | asistent}}\par
	\end{minipage}};
\end{tikzpicture}
}}
{% else
%
\vskip10pt%
{\raggedright\baselineskip= 24pt\titlefont \@title\par}%
\vskip10pt% 21pts between base of title and base of author line
{\raggedright \@author\par}
\vskip8pt% 16pts between base of affiliations and base of dates line 
{\raggedright \datesfont \@ifundefined{@dates}{}{\@dates}\par}
\vskip12pt
{%
\abscontent
}%
\vskip25pt%
}%
%%%
%\@additionalelement
\begin{tikzpicture}[overlay, remember picture]
\path (current page.south west) ++(38.5pt,32pt+25pt+2pt) node[anchor=south west] {\begin{minipage}{2in}
	{\sffamily \@significancestatement}
	\end{minipage}};
\node[anchor=north west] at ([shift={(38.5pt,-32pt-25pt)}]current page.north west) {\includegraphics{FRIteza_logo.pdf}};
%\path[fill=P1797] ([shift={(38.5pt,-32pt-25pt)}]current page.north west) rectangle ([shift={(38.5pt+13mm,-32pt-25pt-13mm)}]current page.north west);
%\node[white,anchor=east] at ([shift={(38.5pt+13mm+1.4mm,-32pt-25pt-13mm/2)}]current page.north west) {\sffamily\bfseries FRI};
%\node[anchor=west] at ([shift={(38.5pt+13mm-1.4mm,-32pt-25pt-13mm/2)}]current page.north west) {\sffamily teza};
\end{tikzpicture}
}%
\vskip\pnas@vertadjust
}%

%% Footnotes set up
\RequirePackage[flushmargin,ragged]{footmisc}
\renewcommand*{\footnotelayout}{\normalfont\sffamily\fontsize{6}{8}\selectfont} % set the footnote font
\renewcommand{\footnoterule}{% Set the footnote hrule style
  \kern -3pt
  {\color{black50} \hrule width 72pt height 0.25pt}
  \kern 2.5pt
}

%% Set up the acknowledgments field
\titleclass{\acknow@section}{straight}[\part]
\newcounter{acknow@section}
\providecommand*{\toclevel@acknow@section}{0}
% Format is set for research articles by default
\titleformat{\acknow@section}[runin]
   {\sffamily\normalsize\bfseries}
   {}
   {0em}
   {#1.}
   []
\titlespacing{\acknow@section}
	{0pt}
	{3.25ex plus 1ex minus .2ex}
	{1.5ex plus .2ex}
\newcommand{\showacknow}{% Display acknowledgments section
\@ifundefined{@acknow}{}{\acknow@section{\iflanguage{slovene}{\textsc{doprinosi}}{\textsc{contributions}}}\small\@acknow}
}

%% Set up the materials&methods field
\titleclass{\matmethods@section}{straight}[\part]
\newcounter{matmethods@section}
\providecommand*{\toclevel@matmethods@section}{0}
% Format is set for research articles by default
\titleformat{\matmethods@section}
   {\sffamily\normalsize\bfseries}
   {}
   {0em}
   {#1}
   []
\titlespacing{\matmethods@section}
	{0pt}
	{3.25ex plus 1ex minus .2ex}
	{1.5ex plus .2ex}
\newcommand{\showmatmethods}{% Display materials&methods section
\@ifundefined{@matmethods}{}{\matmethods@section{\mathmethods@name}{\small\noindent\@matmethods}}
}

%% Other packages
\RequirePackage{enumitem} % For reducing bullet list item separation