using System;

namespace Wilco.SyntaxHighlighting
{
	/// <summary>
	/// Represents an interface to enable scanners to scan source code.
	/// </summary>
	public interface IScanner
	{
		/// <summary>
		/// Gets or sets the child scanner.
		/// </summary>
		/// <remarks>
		/// The child scanner is part of the chain of responsibility. In case a scanner did not find occurrences of the node(s) it 
		/// represents, this scanner may pass the responsibility on to its child.
		/// </remarks>
		IScanner Child
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a collection of dependant <see cref="Wilco.SyntaxHighlighting.IScanner"/> objects.
		/// </summary>
		ScannerCollection Dependencies
		{
			get;
		}

		/// <summary>
		/// Gets or sets whether the scanner is enabled or not.
		/// </summary>
		/// <remarks>
		/// An <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation might want to disable dependant scanners to make sure 
		/// the syntax highlighting will remain valid.
		/// </remarks>
		bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the identification of a scanner.
		/// </summary>
		string ID
		{
			get;
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation class.
		/// </summary>
		/// <param name="tokenizer">The <see cref="Wilco.SyntaxHighlighting.TokenizerBase"/> which is used to tokenize the source code.</param>
		/// <param name="scannerResult">The <see cref="Wilco.SyntaxHighlighting.OccurrenceCollection"/> which will contain the scanner result.</param>
		/// <returns>A new instance of a <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation class.</returns>
		IScanner Create(TokenizerBase tokenizer, OccurrenceCollection scannerResult);

		/// <summary>
		/// Loads the state of the scanner.
		/// </summary>
		/// <param name="o">An <see cref="System.Object"/> that contains the state of the scanner.</param>
		void LoadState(object o);

		/// <summary>
		/// Saves the current state of the scanner.
		/// </summary>
		/// <param name="container">The container which will contain the state.</param>
		/// <returns>An <see cref="System.Object"/> that contains the state of the scanner.</returns>
		object SaveState(object container);

		/// <summary>
		/// Scans a token.
		/// </summary>
		/// <remarks>
		/// An <see cref="Wilco.SyntaxHighlighting.IScanner"/> implementation will generally have a reference to a 
		/// <see cref="Wilco.SyntaxHighlighting.NodeCollection"/> which will be used to store results of a scan.
		/// </remarks>
		/// <param name="token">A token from the source code.</param>
		void Scan(string token);

		/// <summary>
		/// Resets the scanner.
		/// </summary>
		void Reset();
	}
}