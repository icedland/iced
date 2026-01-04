// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation.Cpp {
	sealed class CppDeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public CppDeprecatedWriter( IdentifierConverter idConverter ) =>
			this.idConverter = idConverter;

		public void WriteDeprecated( FileWriter writer, EnumValue value ) {
			var msg = GetDeprecatedString( value );
			if ( msg is not null )
				writer.WriteLine( $"[[deprecated( \"{msg}\" )]]" );
		}

		public void WriteDeprecated( FileWriter writer, Constant value ) {
			var msg = GetDeprecatedString( value );
			if ( msg is not null )
				writer.WriteLine( $"[[deprecated( \"{msg}\" )]]" );
		}

		public string? GetDeprecatedString( EnumValue value ) {
			if ( value.DeprecatedInfo.IsDeprecated ) {
				if ( value.DeprecatedInfo.NewName is not null ) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					return GetDeprecatedString( newValue.Name( idConverter ), value.DeprecatedInfo.Description, true );
				}
				else
					return GetDeprecatedString( null, value.DeprecatedInfo.Description, true );
			}
			else
				return null;
		}

		public string? GetDeprecatedString( Constant value ) {
			if ( value.DeprecatedInfo.IsDeprecated ) {
				if ( value.DeprecatedInfo.NewName is not null ) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					return GetDeprecatedString( newValue.Name( idConverter ), value.DeprecatedInfo.Description, true );
				}
				else
					return GetDeprecatedString( null, value.DeprecatedInfo.Description, true );
			}
			else
				return null;
		}

		string GetDeprecatedString( string? newMember, string? description, bool isMember ) {
			string deprecStr;
			if ( description is not null )
				deprecStr = description;
			else if ( newMember is not null ) {
				if ( isMember )
					deprecStr = $"Use {newMember} instead";
				else
					deprecStr = $"Use {newMember} instead";
			}
			else
				deprecStr = "DEPRECATED. Don't use it!";
			// Escape quotes for C++ string literal
			return deprecStr.Replace( "\"", "\\\"" );
		}
	}
}
