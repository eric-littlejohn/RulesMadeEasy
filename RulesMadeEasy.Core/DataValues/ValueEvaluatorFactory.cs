using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RulesMadeEasy.Core
{
    public class ValueEvaluatorFactory : IValueEvaluatorFactory
    {
        protected readonly IDictionary<Type, IValueEvaluator> _valueEvaluators
            = new Dictionary<Type, IValueEvaluator>();
        
        public IValueEvaluator DefaultValueEvaluator { get; protected set; }

        public ValueEvaluatorFactory(bool registerDefaults = true)
        {
            if (registerDefaults)
            {
                RegisterValueEvaluator<bool>(new BooleanValueEvaluator());
                RegisterValueEvaluator<string>(new StringValueEvaluator(StringComparison.CurrentCulture));
                RegisterValueEvaluator<byte>(new ByteValueEvaluator());
                RegisterValueEvaluator<char>(new CharValueEvaluator());
                RegisterValueEvaluator<decimal>(new DecimalValueEvaluator());
                RegisterValueEvaluator<double>(new DoubleValueEvaluator());
                RegisterValueEvaluator<float>(new FloatValueEvaluator());
                RegisterValueEvaluator<int>(new IntValueEvaluator());
                RegisterValueEvaluator<long>(new LongValueEvaluator());
                RegisterValueEvaluator<short>(new ShortValueEvaluator());
                RegisterValueEvaluator<sbyte>(new SByteValueEvaluator());
                RegisterValueEvaluator<uint>(new UIntValueEvaluator());
                RegisterValueEvaluator<ushort>(new UShortValueEvaluator());
                RegisterValueEvaluator<ulong>(new ULongValueEvaluator());
                RegisterValueEvaluator<Guid>(new GuidValueEvaluator());
                RegisterValueEvaluator<DateTime>(new DateTimeValueEvaluator());
                RegisterValueEvaluator<DateTimeOffset>(new DateTimeOffsetValueEvaluator());

                //Setup default evaluator as object evaluator
                var objEval = new ObjectValueEvaluator();
                RegisterDefaultValueEvaluator(objEval);
                RegisterValueEvaluator<object>(objEval);
            }
        }

        public virtual IValueEvaluator GetValueEvaluator(Type valueType)
            => _valueEvaluators.TryGetValue(valueType, out var matchingEvaluator) ? matchingEvaluator : DefaultValueEvaluator;

        public virtual IReadOnlyDictionary<Type, IValueEvaluator> GetValueEvaluators()
            => new ReadOnlyDictionary<Type, IValueEvaluator>(_valueEvaluators);

        public virtual IValueEvaluatorFactory RegisterValueEvaluator(Type type, IValueEvaluator evaluator)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(evaluator),
                    "Invalid value evaluator provided when registering a default evaluator");
            }

            if (evaluator == null)
            {
                throw new ArgumentNullException(nameof(evaluator),
                    "Invalid value evaluator provided when registering a default evaluator");
            }

            if (!_valueEvaluators.ContainsKey(type))
            {
                _valueEvaluators.Add(type, evaluator);
            }
            else
            {
                _valueEvaluators[type] = evaluator;
            }

            return this;
        }

        public virtual IValueEvaluatorFactory RegisterValueEvaluator<T>(IValueEvaluator evaluator)
            => RegisterValueEvaluator(typeof(T), evaluator);

        public virtual IValueEvaluatorFactory RegisterDefaultValueEvaluator(IValueEvaluator valueEvaluator)
        {
            DefaultValueEvaluator = valueEvaluator ?? 
              throw new ArgumentNullException(nameof(valueEvaluator), 
                  "Invalid value evaluator provided when registering a default evaluator");

            return this;
        }
    }
}