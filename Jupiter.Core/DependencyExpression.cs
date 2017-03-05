using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DependencyExpression
    {
        #region #### VARIABLES ##########################################################
        DependencyExpression _OverrideExpression;
        DependencyObjectContainer _Container;
        DependencyObjectContainer.PropertyValueStorage _ValueStorage;

        /// <summary>
        /// A empty template for wrapping override extensions on a empty property.
        /// </summary>
        static readonly EmptyTemplate Empty = new DependencyExpression.EmptyTemplate();
        #endregion
        #region #### PROPERTIES #########################################################
        /// <summary>
        /// Retrieves the property which is affected by the current expression.
        /// </summary>
        public DependencyProperty Property { get; }
        /// <summary>
        /// Retrieves the <see cref="DependencyExtension"/> which acts as template for the current expression.
        /// </summary>
        public DependencyExtension ExpressionTemplate { get; }
        /// <summary>
        /// Get or set the <see cref="DependencyExpression"/> which overwrites the current.
        /// </summary>
        internal DependencyExpression OverrideExpression
        {
            get { return _OverrideExpression; }
            set
            {
                if (_OverrideExpression != null)
                {

                }
                _OverrideExpression = value;
            }
        }
        /// <summary>
        /// Retrieves the current base value of the <see cref="DependencyExpression"/> which is provided to the target property.
        /// </summary>
        internal Object BaseValue { get; private set; }
        #endregion
        #region #### CTOR ###############################################################
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyExpression"/> class.
        /// </summary>
        /// <param name="property">The <see cref="DependencyProperty"/> which is affected by the current expression.</param>
        /// <param name="expressionTemplate">The <see cref="DependencyExtension"/> which acts as template for the current expression.</param>
        protected DependencyExpression(DependencyProperty property, DependencyExtension expressionTemplate)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            ExpressionTemplate = expressionTemplate ?? throw new ArgumentNullException(nameof(expressionTemplate));
        }
        #endregion
        #region #### PUBLIC METHODS #####################################################
        /// <summary>
        /// Check if the expression is a empty expression.
        /// </summary>
        /// <param name="expression">The expression to test.</param>
        /// <returns>True if the expression is a empty expression; otherwise false.</returns>
        public static Boolean IsEmpty(DependencyExpression expression) => expression is EmptyExpression;
        /// <summary>
        /// Creates a empty <see cref="DependencyExpression"/> which is used for wrapping override extensions on a empty property.
        /// </summary>
        /// <param name="property">The property to create a empty expression from.</param>
        /// <returns>The empty dependency expression.</returns>
        public static DependencyExpression CreateEmpty(DependencyProperty property) => Empty.CreateExpression(property);
        /// <summary>
        /// Initializes the <see cref="DependencyExpression"/> with a <see cref="DependencyObjectContainer"/>.
        /// </summary>
        /// <param name="container">The <see cref="DependencyObjectContainer"/> which</param>
        /// <param name="valueStorage">The <see cref="DependencyObjectContainer.PropertyValueStorage"/> of the value.</param>
        internal void Initialize(DependencyObjectContainer container, DependencyObjectContainer.PropertyValueStorage valueStorage)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _ValueStorage = valueStorage ?? throw new ArgumentNullException(nameof(valueStorage));

            try
            {
                BaseValue = Property.GetPropertyDefault(container.RepresentedObject);
                OnInitialize();
            }
            catch
            {
                // Clear variables
                _Container = null;
                _ValueStorage = null;
                throw;
            }
        }
        /// <summary>
        /// Releases the resources which have been used by the current <see cref="DependencyExpression"/>.
        /// </summary>
        internal void Release()
        {
            try
            {
                OnRelease();
            }
            finally
            {
                // Clear variables
                _Container = null;
                _ValueStorage = null;
            }
        }
        #endregion
        #region #### PRIVATE METHODS ####################################################
        /// <summary>
        /// Changes the value of the expression.
        /// </summary>
        /// <param name="baseValue">The new base value of the property.</param>
        /// <returns>The exception if any exception got thrown during the the assignment process.</returns>
        protected Exception ChangeValue(Object baseValue)
        {
            if (_OverrideExpression == null)
            {
                // Silent fail here instead of throwing a exception might prevent a lot of errors
                //if (_IsDisposed) throw new InvalidOperationException("Markup expression is not longer in use");
                //if (_ChangeCount > 100) throw new PossibleEndlessLoopException().......
                //_ChangeCount++;
                try
                {
                    _ValueStorage.SetMarkupValue(_Container, BaseValue = baseValue);
                    //_Container.SetMarkupValue(Property, baseValue);
                }
                catch (Exception ex)
                {
                    //_ChangeCount--;
                    return ex;
                }
                //_ChangeCount--;
            }

            return null;
        }
        /// <summary>
        /// Initializes the <see cref="DependencyExpression"/>.
        /// </summary>
        protected abstract void OnInitialize();
        /// <summary>
        /// Releases all resources used by the current <see cref="DependencyExpression"/>.
        /// </summary>
        protected abstract void OnRelease();
        #endregion
        #region #### NESTED TYPES #######################################################
        /// <summary>
        /// Represents a template for empty expressions.
        /// </summary>
        sealed class EmptyTemplate : DependencyExtension
        {
            #region #### PRIVATE METHODS ####################################################
            /// <summary>
            /// Creates a new <see cref="DependencyExpression"/> from the current <see cref="DependencyExtension"/> for the specified <see cref="DependencyProperty"/>.
            /// </summary>
            /// <param name="property">The property for which the expression should be created.</param>
            /// <returns>The expression which have been created.</returns>
            protected internal override DependencyExpression CreateExpression(DependencyProperty property) => new EmptyExpression(property, this);
            #endregion
        }
        /// <summary>
        /// Represents a simple expression to allow overwriting a empty expression.
        /// </summary>
        sealed class EmptyExpression : DependencyExpression
        {
            #region #### CTOR ###############################################################
            /// <summary>
            /// Initializes a new instance of the <see cref="EmptyExpression"/> class.
            /// </summary>
            /// <param name="property">The <see cref="DependencyProperty"/> which is affected by the current expression.</param>
            /// <param name="expressionTemplate">The <see cref="DependencyExtension"/> which acts as template for the current expression.</param>
            public EmptyExpression(DependencyProperty property, DependencyExtension expressionTemplate)
                : base(property, expressionTemplate)
            {
            }
            #endregion
            #region #### PRIVATE METHODS ####################################################
            /// <summary>
            /// Initializes the <see cref="DependencyExpression"/>.
            /// </summary>
            protected override void OnInitialize() { }
            /// <summary>
            /// Releases all resources used by the current <see cref="DependencyExpression"/>.
            /// </summary>
            protected override void OnRelease() { }
            #endregion
        }
        #endregion
    }
}
